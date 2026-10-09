using MediCare.Domain.Repositories;
using MediCare.Domain.Rooms;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Persistence.Repositories;

public sealed class RoomRepository(AppDbContext dbContext) : IRoomRepository
{
    public Task<Room> AddAsync(Room room, CancellationToken cancellationToken = default)
    {
        dbContext.Rooms.Add(room);
        return Task.FromResult(room);
    }

    public Task DeleteAsync(Room room, CancellationToken cancellationToken = default)
    {
        dbContext.Rooms.Remove(room);
        return Task.CompletedTask;
    }

    public Task<List<Room>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.Rooms.ToListAsync(cancellationToken);
    }

    public Task<Room?> GetByIdAsync(RoomId id, CancellationToken cancellationToken = default)
    {
        return dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public Task<Room?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return dbContext.Rooms.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }


}
