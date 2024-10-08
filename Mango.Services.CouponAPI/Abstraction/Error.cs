namespace Mango.Services.CouponAPI.Abstraction;

public record Error(string code, string description, int? statusCode)
{
    public static readonly Error None = new Error(string.Empty, string.Empty, null);
}

