using FluentValidation.Results;

namespace Mango.Services.AuthAPI.Helpers;

public static class FluentValidationHelper
{
    public static ValidationResult Validate<TRequest>(AbstractValidator<TRequest> validator, TRequest request)
    {
        return validator.Validate(request);
    }
}