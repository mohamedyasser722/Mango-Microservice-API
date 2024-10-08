using FluentValidation;
using Mango.Services.CouponAPI.Abstraction;
using Mango.Services.CouponAPI.Abstraction.Errors;
using Mango.Services.CouponAPI.Contracts.Coupon;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Services;

public class CouponService(AppDbContext db) : ICouponService
{
    private readonly AppDbContext _db = db;

    public async Task<Result<IEnumerable<Coupon>>> GetAllCoupons()
    {
        var coupons = await _db.Coupons.ToListAsync();

        return Result.Success<IEnumerable<Coupon>>(coupons);
    }

    public async Task<Result<Coupon>> GetCouponById(int couponId)
    {
        var coupon = await _db.Coupons.FirstOrDefaultAsync(u => u.CouponId == couponId);
        if (coupon == null)
            return Result.Failure<Coupon>(CouponError.couponNotFound);

        return Result.Success<Coupon>(coupon);
    }
    public async Task<Result<Coupon>> GetCouponByCode(string couponCode)
    {
        var coupon = await _db.Coupons.FirstOrDefaultAsync(u => u.CouponCode.ToLower() == couponCode.ToLower());
        if (coupon == null)
            return Result.Failure<Coupon>(CouponError.couponNotFound);
        return Result.Success<Coupon>(coupon);
    }

    public async Task<Result<Coupon>> CreateCoupon(CouponRequest couponRequest)
    {
        var validator = new CouponValidator();
        var result = validator.Validate(couponRequest);

        if(!result.IsValid)
            return Result.Failure<Coupon>(new Error(result.Errors.First().ErrorCode, result.Errors.First().ErrorMessage, StatusCodes.Status400BadRequest));

        var coupon = couponRequest.Adapt<Coupon>();

        await _db.Coupons.AddAsync(coupon);
        await _db.SaveChangesAsync();

        return Result.Success<Coupon>(coupon);
    }

}
