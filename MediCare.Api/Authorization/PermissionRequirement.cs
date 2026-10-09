using Microsoft.AspNetCore.Authorization;

namespace MediCare.Api.Authorization;

public sealed record PermissionRequirement(string Permission) : IAuthorizationRequirement;