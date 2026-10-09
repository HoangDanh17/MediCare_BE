using ErrorOr;
using MediatR;
using MediCare.Contract.Major;
using MediCare.Domain.Majors;
using MediCare.Domain.Repositories;

namespace MediCare.Application.Majors.Commands.UpdateMajor;

public sealed class UpdateMajorCommandHandler(
    IMajorRepository majorRepository) : IRequestHandler<UpdateMajorCommand, ErrorOr<MajorResponse>>
{
    public async Task<ErrorOr<MajorResponse>> Handle(UpdateMajorCommand request, CancellationToken cancellationToken)
    {
        var existingMajor = await majorRepository.GetByIdAsync(request.Id, cancellationToken);

        if (existingMajor is null)
        {
            return MajorErrors.NotFound;
        }

        var checkExistingMajor = await majorRepository.GetByNameAsync(request.Name, cancellationToken);

        if (checkExistingMajor is not null && checkExistingMajor.Id != request.Id)
        {
            return MajorErrors.AlreadyExist;
        }

        var updateResult = existingMajor.Update(request.Name, request.Description);

        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }

        return MajorResponse.From(existingMajor);
    }
}
