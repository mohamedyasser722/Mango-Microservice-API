namespace Mango.Services.AuthAPI.Abstraction.Errors;

public class AuthErrors
{
    // not found
    public static readonly Error userNotFound = new Error("user_not_found", "User not found", StatusCodes.Status404NotFound);
    public static readonly Error invalidPassword = new Error("invalid_password", "Invalid password", StatusCodes.Status400BadRequest);
    public static readonly Error invalidEmailOrPassword = new Error("invalid_email_or_password", "Invalid email or password", StatusCodes.Status400BadRequest);
    public static readonly Error invalidToken = new("invalid_token", "The provided token is invalid or expired.", StatusCodes.Status400BadRequest);
    public static readonly Error invalidRefreshToken = new("invalid_refresh_token", "The provided refresh token is invalid or expired.", StatusCodes.Status400BadRequest);
    public static readonly Error userLockedOut = new("user_locked_out", "User is locked out", StatusCodes.Status400BadRequest);
    public static readonly Error roleNotFound = new("role_not_found", "Role not found", StatusCodes.Status404NotFound);
}
