using FluentValidation;
using SubmissionService.Application.DTOs.Appeal;

namespace SubmissionService.Application.Validators.Appeal;

public class UpdateAppealRequestValidator : AbstractValidator<UpdateAppealRequest>
{
    public UpdateAppealRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(x => x.SubmissionId)
            .GreaterThan(0).WithMessage("SubmissionId must be greater than 0.");

        RuleFor(x => x.AppealState)
            .MaximumLength(100).WithMessage("AppealState cannot exceed 100 characters.");

        RuleFor(x => x.CoverLetter)
            .MaximumLength(5000).WithMessage("CoverLetter cannot exceed 5000 characters.");

        RuleFor(x => x.Comment)
            .MaximumLength(5000).WithMessage("Comment cannot exceed 5000 characters.");
    }
}

