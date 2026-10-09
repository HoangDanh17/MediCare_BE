using MediCare.Domain.Majors;

namespace MediCare.Domain.Repositories;

public interface IMajorRepository
{
    Task<List<Major>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Major?> GetByIdAsync(MajorId id, CancellationToken cancellationToken = default);
    Task<Major?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Major> AddAsync(Major major, CancellationToken cancellationToken = default);
    Task DeleteAsync(Major major, CancellationToken cancellationToken = default);
}
