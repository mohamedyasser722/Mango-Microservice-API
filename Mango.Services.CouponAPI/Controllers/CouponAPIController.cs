using Mango.Services.CouponAPI.Abstraction;
using Mango.Services.CouponAPI.Contracts.Coupon;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Services;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;

namespace Mango.Services.CouponAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CouponAPIController(AppDbContext db, ICouponService couponService) : ControllerBase
{
    private readonly AppDbContext _db = db;
    private readonly ICouponService _couponService = couponService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var coupons = await _couponService.GetAllCoupons();

        return Ok(coupons.Value.Adapt<IEnumerable<CouponResponse>>());
    }

    // get by id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var coupon = await _couponService.GetCouponById(id);

        return coupon.IsSuccess ? Ok(coupon.Value.Adapt<CouponResponse>()) : coupon.ToProblem();
    }

    // get by code
    [HttpGet("GetByCode/{code}")]
    public async Task<IActionResult> Get(string code)
    {
        var coupon = await _couponService.GetCouponByCode(code);

        return coupon.IsSuccess ? Ok(coupon.Value.Adapt<CouponResponse>()) : coupon.ToProblem();
    }

    // create Coupon
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CouponRequest couponRequest)
    {
        var result = await _couponService.CreateCoupon(couponRequest);
        
        if(result.IsFailure)
            return result.ToProblem();
        
        return CreatedAtAction(nameof(Get), new { id = result.Value.CouponId }, result.Value);
    }


}
