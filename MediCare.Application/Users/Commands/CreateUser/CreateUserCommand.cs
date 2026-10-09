using MediCare.Application.Common.Messaging;
using MediCare.Contract.User;
using MediCare.Domain.Users;

namespace MediCare.Application.Users.Commands.CreateUser;

public record class CreateUserCommand
    (string FullName, string Email, string Password, string PhoneNumber, string Address, Gender Gender, DateTime Dob) : ICommand<UserResponse>;
