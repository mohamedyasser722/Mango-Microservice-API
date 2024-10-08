using FluentValidation;

namespace Mango.Services.CouponAPI.Contracts.Coupon;

public class CouponValidator : AbstractValidator<CouponRequest>
{
    public CouponValidator()
    {
        RuleFor(p => p.CouponCode).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(p => p.DiscountAmount).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(p => p.MinAmount).NotEmpty().WithMessage("{PropertyName} is required.");
    }
}
