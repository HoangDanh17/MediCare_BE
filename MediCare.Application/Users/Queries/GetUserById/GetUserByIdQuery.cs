using MediCare.Application.Common.Messaging;
using MediCare.Contract.User;

namespace MediCare.Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid Id) : IQuery<UserResponse>;
