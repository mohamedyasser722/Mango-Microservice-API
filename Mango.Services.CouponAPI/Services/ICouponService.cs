using Mango.Services.CouponAPI.Abstraction;
using Mango.Services.CouponAPI.Contracts.Coupon;
using Mango.Services.CouponAPI.Models;

namespace Mango.Services.CouponAPI.Services;

public interface ICouponService
{
    // get All 
    Task<Result<IEnumerable<Coupon>>> GetAllCoupons();
    // get by id
    Task<Result<Coupon>> GetCouponById(int couponId);
    // get by code
    Task<Result<Coupon>> GetCouponByCode(string couponCode);
    // create Coupon
    Task<Result<Coupon>> CreateCoupon(CouponRequest coupon);

}
