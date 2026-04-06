using FluentValidation;
using OrderService.Application.DTOs;

namespace OrderService.Application.Validators
{
    /// <summary>Validation rules for order creation requests.</summary>
    public class CreateOrderRequestValidator : AbstractValidator<OrderDto>
    {
        public CreateOrderRequestValidator()
        {
            
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("User Name is required.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Order must contain at least one item.");

            RuleForEach(x => x.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.ProductName).NotEmpty();
                items.RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("Quantity must be more than 0.");
                items.RuleFor(i => i.Price).GreaterThan(0).WithMessage("Price must be positive.");
            });
        }
    }
}