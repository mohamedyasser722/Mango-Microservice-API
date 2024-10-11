namespace Mango.Services.CouponAPI.Services;

public interface ICouponService
{
    // get All 
    Task<Result<PaginatedList<CouponResponse>>> GetAllCoupons(RequestFilters requestFilters, CancellationToken cancellationToken);
    // get by id
    Task<Result<Coupon>> GetCouponById(int couponId, CancellationToken cancellationToken);
    // get by code
    Task<Result<Coupon>> GetCouponByCode(string couponCode, CancellationToken cancellationToken);
    // create Coupon
    Task<Result<Coupon>> CreateCoupon(CouponRequest coupon, CancellationToken cancellationToken);
    // update Coupon
    Task<Result<Coupon>> UpdateCoupon(int couponId, CouponRequest coupon, CancellationToken cancellationToken);
    // delete Coupon
    Task<Result<bool>> DeleteCoupon(int couponId, CancellationToken cancellationToken);



}
