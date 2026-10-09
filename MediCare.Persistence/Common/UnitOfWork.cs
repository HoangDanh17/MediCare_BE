using DotNetCore.CAP;
using MediCare.Application.Common.Interfaces;
using MediCare.Domain.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace MediCare.Persistence.Common;

public class UnitOfWork(AppDbContext context, ICapPublisher capPublisher) : IUnitOfWork, IAsyncDisposable
{
    private IDbContextTransaction? _transaction;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            throw new InvalidOperationException("Transaction đã được bắt đầu.");

        _transaction = await context.Database.BeginTransactionAsync(capPublisher, autoCommit: false, cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var ownsTransaction = _transaction is null;
        if (ownsTransaction)
            await BeginTransactionAsync(cancellationToken);

        try
        {
            var aggregates = context.ChangeTracker
                .Entries<IAggregateRoot>()
                .Select(e => e.Entity)
                .Where(a => a.DomainEvents.Count > 0)
                .ToList();

            foreach (var aggregate in aggregates)
            {
                foreach (var domainEvent in aggregate.DomainEvents.ToList())
                {
                    await capPublisher.PublishAsync(
                        domainEvent.EventType, domainEvent, cancellationToken: cancellationToken);
                }
            }

            var result = await context.SaveChangesAsync(cancellationToken);

            // Chỉ xóa event sau khi lưu thành công
            foreach (var aggregate in aggregates)
                aggregate.ClearDomainEvents();

            if (ownsTransaction)
                await CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            if (ownsTransaction)
                await RollbackAsync(CancellationToken.None);
            throw;
        }
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null) return;

        try
        {
            await _transaction.CommitAsync(cancellationToken);
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null) return;

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async ValueTask DisposeAsync() => await DisposeTransactionAsync();

    private async Task DisposeTransactionAsync()
    {
        if (_transaction is null) return;

        await _transaction.DisposeAsync();
        _transaction = null;
    }
}
