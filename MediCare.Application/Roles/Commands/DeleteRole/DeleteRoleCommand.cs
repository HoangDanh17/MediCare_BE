using ErrorOr;
using MediCare.Application.Common.Messaging;

namespace MediCare.Application.Roles.Commands.DeleteRole;

public record class DeleteRoleCommand(Guid Id) : ICommand<Success>;
