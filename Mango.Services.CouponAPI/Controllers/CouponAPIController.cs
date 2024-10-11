namespace Mango.Services.CouponAPI.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CouponAPIController(AppDbContext db,
    ICouponService couponService,
    ICouponService cachedCouponService,
    ICouponService inMemoryCacheCouponService) : ControllerBase
{
    private readonly AppDbContext _db = db;
    private readonly ICouponService _couponService = couponService;
    private readonly ICouponService _cachedCouponService = cachedCouponService;
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
        var coupon = await _couponService.GetCouponById(id, cancellationToken);

        return coupon.IsSuccess ? Ok(coupon.Value.Adapt<CouponResponse>()) : coupon.ToProblem();
    }

    // get by code
    [HttpGet("GetByCode/{code}")]
    public async Task<IActionResult> Get(string code, CancellationToken cancellationToken)
    {
        var coupon = await _couponService.GetCouponByCode(code, cancellationToken);

        return coupon.IsSuccess ? Ok(coupon.Value.Adapt<CouponResponse>()) : coupon.ToProblem();
    }

    // create Coupon
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CouponRequest couponRequest, CancellationToken cancellationToken)
    {
        var result = await _couponService.CreateCoupon(couponRequest, cancellationToken);

        if (result.IsFailure)
            return result.ToProblem();

        return CreatedAtAction(nameof(Get), new { id = result.Value.CouponId }, result.Value);
    }

    // update Coupon
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CouponRequest couponRequest, CancellationToken cancellationToken)
    {
        var result = await _couponService.UpdateCoupon(id, couponRequest, cancellationToken);

        if (result.IsFailure)
            return result.ToProblem();

        return NoContent();
    }

}
