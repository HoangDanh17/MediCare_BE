using MediCare.Domain.Rooms;

namespace MediCare.Domain.Repositories;

public interface IRoomRepository
{
    Task<List<Room>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Room?> GetByIdAsync(RoomId id, CancellationToken cancellationToken = default);
    Task<Room?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Room> AddAsync(Room room, CancellationToken cancellationToken = default);
    Task DeleteAsync(Room room, CancellationToken cancellationToken = default);
}
