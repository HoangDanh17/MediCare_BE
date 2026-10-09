using MediCare.Domain.Users;

namespace MediCare.Contract.User;

public sealed record CreateUserRequest(
    string FullName,
    string Email,
    string Password,
    string PhoneNumber,
    string Address,
    Gender Gender,
    DateTime Dob);

public sealed record AssignUserRoleRequest(Guid RoleId);
