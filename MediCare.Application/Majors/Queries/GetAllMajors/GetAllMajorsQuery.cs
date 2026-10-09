using MediCare.Application.Common.Messaging;
using MediCare.Contract.Major;

namespace MediCare.Application.Majors.Queries.GetAllMajors;

public sealed record GetAllMajorsQuery() : IQuery<IReadOnlyList<MajorResponse>>;
