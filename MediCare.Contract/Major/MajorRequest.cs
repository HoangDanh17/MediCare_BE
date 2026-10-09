namespace MediCare.Contract.Major;

public sealed record CreateMajorRequest(string Name, string? Description);

public sealed record UpdateMajorRequest(string Name, string? Description);
