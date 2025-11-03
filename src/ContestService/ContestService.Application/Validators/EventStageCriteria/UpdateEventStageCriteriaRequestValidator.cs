using ContestService.Application.DTOs.EventStageCriteria;
using FluentValidation;

namespace ContestService.Application.Validators.EventStageCriteria;

public class UpdateEventStageCriteriaRequestValidator : AbstractValidator<UpdateEventStageCriteriaRequest>
{
    public UpdateEventStageCriteriaRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

        RuleFor(x => x.EventStageId)
            .GreaterThan(0).WithMessage("EventStageId must be greater than 0.");
    }
}

