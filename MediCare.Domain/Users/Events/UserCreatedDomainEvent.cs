using MediCare.Domain.Common;
using MediCare.Domain.Roles;

namespace MediCare.Domain.Users.Events;

public sealed record class UserCreatedDomainEvent
    (UserId Id, RoleId RoleId, string FullName, string Email, string PasswordHash,
    string PhoneNumber, string Address, Gender Gender, DateTime Dob, bool IsActive,
    DateTime CreatedAt, DateTime OccurredOnUtc) : IDomainEvent
{
    public string EventType => UserEventTypes.UserCreated;
}
