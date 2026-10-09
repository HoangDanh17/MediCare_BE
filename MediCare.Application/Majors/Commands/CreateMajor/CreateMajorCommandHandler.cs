using ErrorOr;
using MediatR;
using MediCare.Contract.Major;
using MediCare.Domain.Majors;
using MediCare.Domain.Repositories;

namespace MediCare.Application.Majors.Commands.CreateMajor;

public sealed class CreateMajorCommandHandler(
    IMajorRepository majorRepository
) : IRequestHandler<CreateMajorCommand, ErrorOr<MajorResponse>>
{
    public async Task<ErrorOr<MajorResponse>> Handle(CreateMajorCommand request, CancellationToken cancellationToken)
    {
        var existingMajor = await majorRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existingMajor is not null)
        {
            return MajorErrors.AlreadyExist;
        }

        var major = Major.Create(request.Name, request.Description);

        if (major.IsError)
        {
            return major.Errors;
        }

        await majorRepository.AddAsync(major.Value, cancellationToken);

        return MajorResponse.From(major.Value);
    }
}
