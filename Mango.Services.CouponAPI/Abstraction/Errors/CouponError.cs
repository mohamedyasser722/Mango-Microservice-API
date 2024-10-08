namespace Mango.Services.CouponAPI.Abstraction.Errors;

public static class CouponError
{
    public static readonly Error couponNotFound = new Error("coupon_not_found", "Coupon not found", StatusCodes.Status404NotFound);
}
