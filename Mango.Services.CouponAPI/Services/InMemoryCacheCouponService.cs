namespace Mango.Services.CouponAPI.Services;

public class InMemoryCacheCouponService(AppDbContext db, ICacheService cacheService) : ICouponService
{
    private readonly AppDbContext _db = db;
    private readonly ICacheService _cacheService = cacheService;

    public async Task<Result<PaginatedList<CouponResponse>>> GetAllCoupons(RequestFilters requestFilters, CancellationToken cancellationToken)
    {
        // Apply Caching
        var cacheKey = $"GetAllCoupons_{requestFilters.PageNumber}_{requestFilters.PageSize}_{requestFilters.SearchValue}_{requestFilters.SortColumn}_{requestFilters.SortDirection}";

        var cachedCoupons = await _cacheService.GetAsync<PaginatedList<CouponResponse>>(cacheKey);
        if (cachedCoupons != null)
            return Result.Success(cachedCoupons);

        // fetch data from db
        var coupons = _db.Coupons.AsNoTracking();

        if (!string.IsNullOrEmpty(requestFilters.SearchValue))
            coupons = coupons.Where(c => c.CouponCode.ToLower().Contains(requestFilters.SearchValue));
        if (!string.IsNullOrEmpty(requestFilters.SortColumn))
            coupons = coupons.OrderBy($"{requestFilters.SortColumn} {requestFilters.SortDirection}");

        var source = coupons.ProjectToType<CouponResponse>();

        var paginatedCoupons = await PaginatedList<CouponResponse>.Create(source, requestFilters.PageNumber!.Value, requestFilters.PageSize!.Value, cancellationToken);

        // Store the result in cache
        await _cacheService.SetAsync(cacheKey, paginatedCoupons, 60, cancellationToken);

        return Result.Success(paginatedCoupons);
    }

    public async Task<Result<Coupon>> GetCouponById(int couponId, CancellationToken cancellationToken)
    {
        // Apply Caching
        var cacheKey = $"GetCouponById_{couponId}";

        var cachedCoupon = await _cacheService.GetAsync<Coupon>(cacheKey);
        if (cachedCoupon != null)
            return Result.Success(cachedCoupon);

        var coupon = await _db.Coupons.FirstOrDefaultAsync(u => u.CouponId == couponId, cancellationToken);
        if (coupon == null)
            return Result.Failure<Coupon>(CouponError.couponNotFound);

        // Store the result in cache
        await _cacheService.SetAsync(cacheKey, coupon, 60, cancellationToken);

        return Result.Success<Coupon>(coupon);
    }
    public async Task<Result<Coupon>> GetCouponByCode(string couponCode, CancellationToken cancellationToken)
    {
        // Apply Caching
        var cacheKey = $"GetCouponByCode_{couponCode}";

        var cachedCoupon = await _cacheService.GetAsync<Coupon>(cacheKey);
        if (cachedCoupon != null)
            return Result.Success(cachedCoupon);

        var coupon = await _db.Coupons.FirstOrDefaultAsync(u => u.CouponCode.ToLower() == couponCode.ToLower(), cancellationToken);
        if (coupon == null)
            return Result.Failure<Coupon>(CouponError.couponNotFound);

        await _cacheService.SetAsync(cacheKey, coupon, 5, cancellationToken);

        return Result.Success<Coupon>(coupon);
    }

    public async Task<Result<Coupon>> CreateCoupon(CouponRequest couponRequest, CancellationToken cancellationToken)
    {
        var validator = new CouponValidator();
        var result = validator.Validate(couponRequest);

        if (!result.IsValid)
            return Result.Failure<Coupon>(new Error(result.Errors.First().ErrorCode, result.Errors.First().ErrorMessage, StatusCodes.Status400BadRequest));

        var coupon = couponRequest.Adapt<Coupon>();

        await _db.Coupons.AddAsync(coupon, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        // Remove the cache
        await _cacheService.RemoveAsync("GetAllCoupons");


        return Result.Success<Coupon>(coupon);
    }

    public async Task<Result<Coupon>> UpdateCoupon(int couponId, CouponRequest couponRequest, CancellationToken cancellationToken)
    {
        var validator = new CouponValidator();
        var result = validator.Validate(couponRequest);

        if (!result.IsValid)
            return Result.Failure<Coupon>(new Error(result.Errors.First().ErrorCode, result.Errors.First().ErrorMessage, StatusCodes.Status400BadRequest));

        var coupon = await _db.Coupons.FirstOrDefaultAsync(u => u.CouponId == couponId, cancellationToken);
        if (coupon == null)
            return Result.Failure<Coupon>(CouponError.couponNotFound);


        coupon = couponRequest.Adapt(coupon);

        await _db.SaveChangesAsync(cancellationToken);

        // Remove the cache
        await _cacheService.RemoveKeysStartingWith("GetAllCoupons_");
        await _cacheService.RemoveAsync($"GetCouponById_{couponId}");

        return Result.Success<Coupon>(coupon);
    }

}
