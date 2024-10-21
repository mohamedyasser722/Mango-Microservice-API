using FluentValidation.Results;

namespace Mango.Services.CouponAPI.Abstraction;

public record Error(string code, string description, int? statusCode)
{
    public static readonly Error None = new Error(string.Empty, string.Empty, null);

    public static Error FromValidationResult(ValidationResult validationResult)
    {
        var firstError = validationResult.Errors.First();
        return new Error(firstError.ErrorCode, firstError.ErrorMessage, StatusCodes.Status400BadRequest);
    }
}

