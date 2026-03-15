using Amazon.Application.DTOs;
using FluentValidation;

namespace Amazon.Application.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductDTO>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .Length(5, 500).WithMessage("Description must be between 5 and 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .LessThanOrEqualTo(1000000).WithMessage("Price cannot exceed 1,000,000.");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.")
                .LessThanOrEqualTo(10000).WithMessage("Stock quantity cannot exceed 10,000.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("A valid CategoryId is required.");
        }
    }

    public class UpdateProductValidator : AbstractValidator<UpdateProductDTO>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .LessThanOrEqualTo(1000000).WithMessage("Price cannot exceed 1,000,000.");
        }
    }
}
