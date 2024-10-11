namespace Mango.Services.CouponAPI.Cache;

public class DecoratorCachedCouponService(ICouponService couponService,
    IMemoryCache memoryCache,
    ICacheService cacheService,
    ILogger<DecoratorCachedCouponService> logger) : ICouponService
{
    private readonly ICouponService _decorated = couponService;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly ICacheService _cacheService = cacheService;
    private readonly ILogger<DecoratorCachedCouponService> logger = logger;

    // Cache duration (5 minutes in this example)
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

    public Task<Result<Coupon>> CreateCoupon(CouponRequest coupon, CancellationToken cancellationToken = default)
    {
        return _decorated.CreateCoupon(coupon, cancellationToken);
    }

    public async Task<Result<PaginatedList<CouponResponse>>> GetAllCoupons(RequestFilters requestFilters, CancellationToken cancellationToken = default)
    {
        // Generate cache key based on filters to make sure we cache based on request parameters
        string cacheKey = $"GetAllCoupons_{requestFilters.PageNumber}_{requestFilters.PageSize}_{requestFilters.SearchValue}_{requestFilters.SortColumn}_{requestFilters.SortDirection}";

        // Check if data is already in cache
        if (!_memoryCache.TryGetValue(cacheKey, out Result<PaginatedList<CouponResponse>> cachedCoupons))
        {
            logger.LogInformation($"fetch GetAllCoupons from database");
            // If not, get the data from the decorated service (CouponService)
            cachedCoupons = await _decorated.GetAllCoupons(requestFilters, cancellationToken);

            // Store the result in cache
            await _cacheService.SetAsync(cacheKey, cachedCoupons, 5, cancellationToken);
        }

        return cachedCoupons;
    }

    // another way to implement GetAllCoupons using GetOrCreateAsync

    //public async Task<Result<PaginatedList<CouponResponse>>> GetAllCoupons(RequestFilters requestFilters, CancellationToken cancellationToken = default)
    //{
    //    // Generate cache key based on filters to make sure we cache based on request parameters
    //    string cacheKey = $"GetAllCoupons_{requestFilters.PageNumber}_{requestFilters.PageSize}_{requestFilters.SearchValue}_{requestFilters.SortColumn}_{requestFilters.SortDirection}";

    //    return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
    //    {
    //        entry.AbsoluteExpirationRelativeToNow = _cacheDuration;

    //        return await _decorated.GetAllCoupons(requestFilters, cancellationToken);
    //    });
    //}

    public async Task<Result<Coupon>> GetCouponByCode(string couponCode, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"GetCouponByCode_{couponCode}";

        if (!_memoryCache.TryGetValue(cacheKey, out Result<Coupon> cachedCoupon))
        {
            cachedCoupon = await _decorated.GetCouponByCode(couponCode, cancellationToken);
            await _cacheService.SetAsync(cacheKey, cachedCoupon, 5, cancellationToken);
        }

        return cachedCoupon;
    }

    public async Task<Result<Coupon>> GetCouponById(int couponId, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"GetCouponById_{couponId}";

        if (!_memoryCache.TryGetValue(cacheKey, out Result<Coupon> cachedCoupon))
        {
            cachedCoupon = await _decorated.GetCouponById(couponId, cancellationToken);
            _memoryCache.Set(cacheKey, cachedCoupon, _cacheDuration);
        }

        return cachedCoupon;
    }

    public async Task<Result<Coupon>> UpdateCoupon(int couponId, CouponRequest coupon, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"GetCouponById_{couponId}";
        await _cacheService.RemoveAsync(cacheKey);
        await _cacheService.RemoveKeysStartingWith("GetAllCoupons_");
        return await _decorated.UpdateCoupon(couponId, coupon, cancellationToken);
    }
}
