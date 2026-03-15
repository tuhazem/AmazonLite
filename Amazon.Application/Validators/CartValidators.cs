using Amazon.Application.DTOs;
using FluentValidation;

namespace Amazon.Application.Validators
{
    public class AddCartItemValidator : AbstractValidator<AddCartItemDTO>
    {
        public AddCartItemValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("A valid ProductId is required.");

            RuleFor(x => x.Quantity)
                .InclusiveBetween(1, 100).WithMessage("Quantity must be between 1 and 100.");
        }
    }

    public class UpdateCartItemValidator : AbstractValidator<UpdateCartItemDTO>
    {
        public UpdateCartItemValidator()
        {
            RuleFor(x => x.Quantity)
                .InclusiveBetween(1, 100).WithMessage("Quantity must be between 1 and 100.");
        }
    }
}
