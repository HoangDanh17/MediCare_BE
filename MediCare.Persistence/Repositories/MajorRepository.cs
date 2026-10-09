using MediCare.Domain.Majors;
using MediCare.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Persistence.Repositories;

public sealed class MajorRepository(AppDbContext dbContext) : IMajorRepository
{
    public Task<Major> AddAsync(Major major, CancellationToken cancellationToken = default)
    {
        dbContext.Majors.Add(major);
        return Task.FromResult(major);
    }

    public Task DeleteAsync(Major major, CancellationToken cancellationToken = default)
    {
        dbContext.Majors.Remove(major);
        return Task.CompletedTask;
    }

    public Task<List<Major>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.Majors.ToListAsync(cancellationToken);
    }

    public Task<Major?> GetByIdAsync(MajorId id, CancellationToken cancellationToken = default)
    {
        return dbContext.Majors.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public Task<Major?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return dbContext.Majors.FirstOrDefaultAsync(m => m.Name == name, cancellationToken);
    }
}
