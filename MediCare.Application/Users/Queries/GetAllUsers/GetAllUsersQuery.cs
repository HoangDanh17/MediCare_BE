using MediCare.Application.Common.Messaging;
using MediCare.Contract.Common;
using MediCare.Contract.User;

namespace MediCare.Application.Users.Queries.GetAllUsers;

public sealed record GetAllUsersQuery(int Page = 1, int PageSize = 10, string? Search = null)
    : IQuery<PagedResponse<UserResponse>>;
