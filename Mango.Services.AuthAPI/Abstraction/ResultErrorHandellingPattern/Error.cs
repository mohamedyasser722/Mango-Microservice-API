

using FluentValidation.Results;

namespace Mango.Services.AuthAPI.Abstraction.ResultErrorHandellingPattern;

public record Error(string Code, string Description, int? StatusCode)
{
    public static readonly Error None = new Error(string.Empty, string.Empty, null);

    public static Error FromValidationResult(ValidationResult validationResult)
    {
        var firstError = validationResult.Errors.First();
        return new Error(firstError.ErrorCode, firstError.ErrorMessage, StatusCodes.Status400BadRequest);
    }

    public static Error FromIdentityResult (IdentityResult identityResult)
    {
        var firstError = identityResult.Errors.First();
        return new Error(firstError.Code, firstError.Description, StatusCodes.Status400BadRequest);
    }
}

