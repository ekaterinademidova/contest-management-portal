using ContestService.Application.DTOs.EventStage;
using FluentValidation;

namespace ContestService.Application.Validators.EventStage;

public class CreateEventStageRequestValidator : AbstractValidator<CreateEventStageRequest>
{
    public CreateEventStageRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

        RuleFor(x => x.EventId)
            .GreaterThan(0).WithMessage("EventId must be greater than 0.");

        RuleFor(x => x.DateEnd)
            .GreaterThan(x => x.DateStart).WithMessage("DateEnd must be greater than DateStart.");
    }
}

