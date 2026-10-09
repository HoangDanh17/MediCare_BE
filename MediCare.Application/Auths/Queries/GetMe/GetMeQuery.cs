using MediCare.Application.Common.Messaging;
using MediCare.Contract.User;

namespace MediCare.Application.Auths.Queries.GetMe;

public sealed record GetMeQuery(Guid UserId) : IQuery<UserResponse>;
