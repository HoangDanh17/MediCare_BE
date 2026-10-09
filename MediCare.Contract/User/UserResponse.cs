using MediCare.Domain.Users;
using DomainUser = MediCare.Domain.Users.User;

namespace MediCare.Contract.User;

public class UserResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public Gender Gender { get; set; }
    public DateTime Dob { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public static UserResponse From(DomainUser user, string roleName) => new()
    {
        Id = user.Id.Value,
        FullName = user.FullName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        Address = user.Address,
        Gender = user.Gender,
        Dob = user.Dob,
        RoleId = user.RoleId.Value,
        RoleName = roleName,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt,
    };
}
