using ErrorOr;
using MediatR;
using MediCare.Application.Common.Interfaces;
using MediCare.Application.Common.Messaging;

namespace MediCare.Application.Common.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, ErrorOr<TResponse>>
     where TRequest : ICommand<TResponse>
{
    public async Task<ErrorOr<TResponse>> Handle
        (TRequest request,
        RequestHandlerDelegate<ErrorOr<TResponse>> next,
        CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await next(cancellationToken);

            if (result.IsError)
            {
                await unitOfWork.RollbackAsync(cancellationToken);
                return result;
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            await unitOfWork.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
}
