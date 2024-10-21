using FluentValidation;

namespace Mango.Service.ProductAPI.Contracts.Product;

public class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(p => p.Price).NotEmpty().WithMessage("Price is required.").InclusiveBetween(1, 1000).WithMessage("Price Must be between 1 and 1000.");

       
    }
}
