using ContestService.Application.DTOs.Attachment;
using FluentValidation;

namespace ContestService.Application.Validators.Attachment;

public class UpdateAttachmentRequestValidator : AbstractValidator<UpdateAttachmentRequest>
{
    public UpdateAttachmentRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");
    }
}

