namespace Mango.Services.CouponAPI.Contracts.Coupon;

public record CouponResponse(
 int CouponId,
string CouponCode,
double DiscountAmount,
 int MinAmount

);