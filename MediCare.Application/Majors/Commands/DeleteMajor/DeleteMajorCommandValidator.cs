using FluentValidation;

namespace MediCare.Application.Majors.Commands.DeleteMajor;

public sealed class DeleteMajorCommandValidator : AbstractValidator<DeleteMajorCommand>
{
    public DeleteMajorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Major Id is required.");
    }
}
