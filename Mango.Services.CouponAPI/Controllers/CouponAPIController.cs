namespace Mango.Services.CouponAPI.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CouponAPIController(ICouponService inMemoryCacheCouponService) : ControllerBase
{
    private readonly ICouponService _inMemoryCacheCouponService = inMemoryCacheCouponService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters requestFilters, CancellationToken cancellationToken)
    {
        var coupons = await _inMemoryCacheCouponService.GetAllCoupons(requestFilters, cancellationToken);
        return Ok(coupons.Value);
    }

    // get by id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var coupon = await _inMemoryCacheCouponService.GetCouponById(id, cancellationToken);

        return coupon.IsSuccess ? Ok(coupon.Value.Adapt<CouponResponse>()) : coupon.ToProblem();
    }

    // get by code
    [HttpGet("GetByCode/{code}")]
    public async Task<IActionResult> Get(string code, CancellationToken cancellationToken)
    {
        var coupon = await _inMemoryCacheCouponService.GetCouponByCode(code, cancellationToken);

        return coupon.IsSuccess ? Ok(coupon.Value.Adapt<CouponResponse>()) : coupon.ToProblem();
    }

    // create Coupon
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CouponRequest couponRequest, CancellationToken cancellationToken)
    {
        var result = await _inMemoryCacheCouponService.CreateCoupon(couponRequest, cancellationToken);

        if (result.IsFailure)
            return result.ToProblem();

        return CreatedAtAction(nameof(Get), new { id = result.Value.CouponId }, result.Value);
    }

    // update Coupon
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CouponRequest couponRequest, CancellationToken cancellationToken)
    {
        var result = await _inMemoryCacheCouponService.UpdateCoupon(id, couponRequest, cancellationToken);

        if (result.IsFailure)
            return result.ToProblem();

        return NoContent();
    }

    // delete Coupon
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _inMemoryCacheCouponService.DeleteCoupon(id, cancellationToken);

        if (result.IsFailure)
            return result.ToProblem();

        return NoContent();
    }
}
