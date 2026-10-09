using MediCare.Application.Common.Messaging;
using MediCare.Contract.Major;

namespace MediCare.Application.Majors.Commands.CreateMajor;

public sealed record CreateMajorCommand(string Name, string? Description) : ICommand<MajorResponse>;
