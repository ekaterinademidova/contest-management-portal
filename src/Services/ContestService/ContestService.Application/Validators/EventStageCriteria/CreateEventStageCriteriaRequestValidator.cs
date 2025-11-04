using ContestService.Application.DTOs.EventStageCriteria;
using FluentValidation;

namespace ContestService.Application.Validators.EventStageCriteria;

public class CreateEventStageCriteriaRequestValidator : AbstractValidator<CreateEventStageCriteriaRequest>
{
    public CreateEventStageCriteriaRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

        RuleFor(x => x.EventStageId)
            .GreaterThan(0).WithMessage("EventStageId must be greater than 0.");
    }
}

