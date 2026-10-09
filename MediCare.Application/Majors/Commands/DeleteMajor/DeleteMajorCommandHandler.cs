using ErrorOr;
using MediatR;
using MediCare.Domain.Majors;
using MediCare.Domain.Repositories;

namespace MediCare.Application.Majors.Commands.DeleteMajor;

public sealed class DeleteMajorCommandHandler(
    IMajorRepository majorRepository) : IRequestHandler<DeleteMajorCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(DeleteMajorCommand request, CancellationToken cancellationToken)
    {
        var checkMajor = await majorRepository.GetByIdAsync(request.Id);
        if (checkMajor is null)
        {
            return MajorErrors.NotFound;
        }

        await majorRepository.DeleteAsync(checkMajor, cancellationToken);

        return Result.Success;
    }
}
