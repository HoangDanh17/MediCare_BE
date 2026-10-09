using ErrorOr;
using MediatR;
using MediCare.Contract.Major;
using MediCare.Domain.Repositories;

namespace MediCare.Application.Majors.Queries.GetAllMajors;

public sealed class GetAllMajorsQueryHandler(
    IMajorRepository majorRepository) : IRequestHandler<GetAllMajorsQuery, ErrorOr<IReadOnlyList<MajorResponse>>>
{
    public async Task<ErrorOr<IReadOnlyList<MajorResponse>>> Handle(GetAllMajorsQuery request, CancellationToken cancellationToken)
    {
        var majors = await majorRepository.GetAllAsync(cancellationToken);

        return majors
            .OrderBy(m => m.Name)
            .Select(MajorResponse.From)
            .ToList();
    }
}
