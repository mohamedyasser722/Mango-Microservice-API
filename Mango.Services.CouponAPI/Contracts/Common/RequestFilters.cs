namespace Mango.Services.CouponAPI.Contracts.Common;

public class RequestFilters
{
    private const int MaxPageSize = 100;

    private int _pageSize = 10;

    private int? _pageNumber = 1;




    public int? PageNumber
    {
        get => _pageNumber ?? 1;
        init => _pageNumber = value;
    }

    public int? PageSize
    {
        get => _pageSize;
        init => _pageSize = (value > MaxPageSize) ? MaxPageSize : value ?? 10; // Handle null value
    }

    public string? SearchValue { get; init; }
    public string? SortDirection { get; init; } = "ASC";
    public string? SortColumn { get; init; }
}
