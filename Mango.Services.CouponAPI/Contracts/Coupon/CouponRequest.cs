namespace Mango.Services.CouponAPI.Contracts.Coupon;

public record CouponRequest
(
    string CouponCode,
    double DiscountAmount,
    int MinAmount
);
