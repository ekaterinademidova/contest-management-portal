using ContestService.Application.DTOs.ContestDocsPackage;
using FluentValidation;

namespace ContestService.Application.Validators.ContestDocsPackage;

public class CreateContestDocsPackageRequestValidator : AbstractValidator<CreateContestDocsPackageRequest>
{
    public CreateContestDocsPackageRequestValidator()
    {
        RuleFor(x => x.ContestNoticeId)
            .GreaterThan(0).WithMessage("ContestNoticeId must be greater than 0.");

        RuleFor(x => x.AttachmentId)
            .GreaterThan(0).WithMessage("AttachmentId must be greater than 0.");
    }
}

