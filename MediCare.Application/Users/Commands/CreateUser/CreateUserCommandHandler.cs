using ErrorOr;
using MediatR;
using MediCare.Application.Common.Interfaces;
using MediCare.Contract.User;
using MediCare.Domain.Repositories;
using MediCare.Domain.Roles;
using MediCare.Domain.Users;

namespace MediCare.Application.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IPasswordHasher passwordHasher
    ) : IRequestHandler<CreateUserCommand, ErrorOr<UserResponse>>
{
    public async Task<ErrorOr<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var checkExistEmail = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (checkExistEmail is not null)
        {
            return UserErrors.DuplicateEmail;
        }

        var passwordHash = passwordHasher.HashPassword(request.Password);

        var role = await roleRepository.GetByNameAsync(RoleNames.User, cancellationToken);

        if (role is null)
        {
            return RoleErrors.NotFound;
        }

        var result = User.Create(
            role.Id,
            request.FullName,
            request.Email,
            passwordHash,
            request.PhoneNumber,
            request.Address,
            request.Gender,
            request.Dob
        );

        if (result.IsError)
            return result.Errors;

        await userRepository.AddAsync(result.Value, cancellationToken);

        return UserResponse.From(result.Value, role.Name);
    }
}
