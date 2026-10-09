using ErrorOr;
using MediatR;
using MediCare.Contract.Major;
using MediCare.Domain.Majors;
using MediCare.Domain.Repositories;

namespace MediCare.Application.Majors.Queries.GetMajorById;

public sealed class GetMajorByIdQueryHandler(
    IMajorRepository majorRepository) : IRequestHandler<GetMajorByIdQuery, ErrorOr<MajorResponse>>
{
    public async Task<ErrorOr<MajorResponse>> Handle(GetMajorByIdQuery request, CancellationToken cancellationToken)
    {
        var major = await majorRepository.GetByIdAsync(new MajorId(request.Id), cancellationToken);

        if (major is null)
            return MajorErrors.NotFound;

        return MajorResponse.From(major);
    }
}
