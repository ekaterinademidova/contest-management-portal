using ContestService.Application.DTOs.Submission;
using FluentValidation;

namespace ContestService.Application.Validators.Submission;

public class UpdateSubmissionRequestValidator : AbstractValidator<UpdateSubmissionRequest>
{
    public UpdateSubmissionRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(x => x.ContestNoticeId)
            .GreaterThan(0).WithMessage("ContestNoticeId must be greater than 0.");

        RuleFor(x => x.ParticipantId)
            .GreaterThan(0).WithMessage("ParticipantId must be greater than 0.");

        RuleFor(x => x.SubmissionState)
            .MaximumLength(100).WithMessage("SubmissionState cannot exceed 100 characters.");

        RuleFor(x => x.CoverLetter)
            .MaximumLength(5000).WithMessage("CoverLetter cannot exceed 5000 characters.");

        RuleFor(x => x.Comment)
            .MaximumLength(5000).WithMessage("Comment cannot exceed 5000 characters.");
    }
}

