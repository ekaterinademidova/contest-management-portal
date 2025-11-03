using ContestService.Application.DTOs.ContestNotice;
using FluentValidation;

namespace ContestService.Application.Validators.ContestNotice;

public class CreateContestNoticeRequestValidator : AbstractValidator<CreateContestNoticeRequest>
{
    public CreateContestNoticeRequestValidator()
    {
        RuleFor(x => x.OrganizerId)
            .GreaterThan(0).WithMessage("OrganizerId must be greater than 0.");

        RuleFor(x => x.EventId)
            .GreaterThan(0).WithMessage("EventId must be greater than 0.");

        RuleFor(x => x.CompetitionType)
            .MaximumLength(100).WithMessage("CompetitionType cannot exceed 100 characters.");
    }
}

