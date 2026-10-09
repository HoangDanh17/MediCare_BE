using ErrorOr;
using MediCare.Application.Common.Messaging;
using MediCare.Domain.Majors;

namespace MediCare.Application.Majors.Commands.DeleteMajor;

public sealed record DeleteMajorCommand(MajorId Id) : ICommand<Success>;