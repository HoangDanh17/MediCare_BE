using MediCare.Application.Common.Messaging;
using MediCare.Contract.Major;

namespace MediCare.Application.Majors.Queries.GetMajorById;

public sealed record GetMajorByIdQuery(Guid Id) : IQuery<MajorResponse>;
