using ContestService.Application.DTOs.ContestDocsPackage;
using FluentValidation;

namespace ContestService.Application.Validators.ContestDocsPackage;

public class UpdateContestDocsPackageRequestValidator : AbstractValidator<UpdateContestDocsPackageRequest>
{
    public UpdateContestDocsPackageRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(x => x.ContestNoticeId)
            .GreaterThan(0).WithMessage("ContestNoticeId must be greater than 0.");

        RuleFor(x => x.AttachmentId)
            .GreaterThan(0).WithMessage("AttachmentId must be greater than 0.");
    }
}

