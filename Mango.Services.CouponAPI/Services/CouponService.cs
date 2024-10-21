namespace Mango.Services.CouponAPI.Services;

public class CouponService(AppDbContext db) : ICouponService
{
    private readonly AppDbContext _db = db;

    public async Task<Result<PaginatedList<CouponResponse>>> GetAllCoupons(RequestFilters requestFilters, CancellationToken cancellationToken)
    {

        var coupons = _db.Coupons.AsNoTracking();

        if (!string.IsNullOrEmpty(requestFilters.SearchValue))
            coupons = coupons.Where(c => c.CouponCode.ToLower().Contains(requestFilters.SearchValue));
        if (!string.IsNullOrEmpty(requestFilters.SortColumn))
            coupons = coupons.OrderBy($"{requestFilters.SortColumn} {requestFilters.SortDirection}");

        var source = coupons.ProjectToType<CouponResponse>();

        var paginatedCoupons = await PaginatedList<CouponResponse>.Create(source, requestFilters.PageNumber!.Value, requestFilters.PageSize!.Value, cancellationToken);

        return Result.Success(paginatedCoupons);
    }

    public async Task<Result<Coupon>> GetCouponById(int couponId, CancellationToken cancellationToken)
    {
        var coupon = await _db.Coupons.FirstOrDefaultAsync(u => u.CouponId == couponId, cancellationToken);
        if (coupon == null)
            return Result.Failure<Coupon>(CouponError.couponNotFound);

        return Result.Success<Coupon>(coupon);
    }
    public async Task<Result<Coupon>> GetCouponByCode(string couponCode, CancellationToken cancellationToken)
    {
        var coupon = await _db.Coupons.FirstOrDefaultAsync(u => u.CouponCode.ToLower() == couponCode.ToLower(), cancellationToken);
        if (coupon == null)
            return Result.Failure<Coupon>(CouponError.couponNotFound);
        return Result.Success<Coupon>(coupon);
    }

    public async Task<Result<Coupon>> CreateCoupon(CouponRequest couponRequest, CancellationToken cancellationToken)
    {
        var validator = new CouponValidator();
        var result = validator.Validate(couponRequest);

        if (!result.IsValid)
            return Result.Failure<Coupon>(Error.FromValidationResult(result));

        var coupon = couponRequest.Adapt<Coupon>();

        await _db.Coupons.AddAsync(coupon, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return Result.Success<Coupon>(coupon);
    }

    public async Task<Result<Coupon>> UpdateCoupon(int couponId, CouponRequest couponRequest, CancellationToken cancellationToken)
    {
        var validator = new CouponValidator();
        var result = validator.Validate(couponRequest);

        if (!result.IsValid)
            return Result.Failure<Coupon>(Error.FromValidationResult(result));

        var coupon = await _db.Coupons.FirstOrDefaultAsync(u => u.CouponId == couponId, cancellationToken);
        if (coupon == null)
            return Result.Failure<Coupon>(CouponError.couponNotFound);


        coupon = couponRequest.Adapt(coupon);

        await _db.SaveChangesAsync(cancellationToken);

        return Result.Success<Coupon>(coupon);
    }

    public async Task<Result<bool>> DeleteCoupon(int couponId, CancellationToken cancellationToken)
    {
        var coupon = await _db.Coupons.FirstOrDefaultAsync(u => u.CouponId == couponId, cancellationToken);
        if (coupon == null)
            return Result.Failure<bool>(CouponError.couponNotFound);

        _db.Coupons.Remove(coupon);
        await _db.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }

}
