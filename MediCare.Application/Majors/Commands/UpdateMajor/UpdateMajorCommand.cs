using MediCare.Application.Common.Messaging;
using MediCare.Contract.Major;
using MediCare.Domain.Majors;

namespace MediCare.Application.Majors.Commands.UpdateMajor;

public sealed record UpdateMajorCommand(MajorId Id, string Name, string? Description) : ICommand<MajorResponse>;
