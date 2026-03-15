using Amazon.Application.DTOs;
using FluentValidation;

namespace Amazon.Application.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDTO>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .Length(3, 50).WithMessage("Category name must be between 3 and 50 characters.");
        }
    }

    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDTO>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .Length(3, 50).WithMessage("Category name must be between 3 and 50 characters.");
        }
    }
}
