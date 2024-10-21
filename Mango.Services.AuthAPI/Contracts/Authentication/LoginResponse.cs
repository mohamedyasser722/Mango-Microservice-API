namespace Mango.Services.AuthAPI.Contracts.Authentication;

public record LoginResponse
{
    public UserResponse User { get; init; }
    public string Token { get; init; }
    public double ExpiresIn { get; init; }
    public string RefreshToken { get; init; }
    public DateTime RefreshTokenExpiration { get; init; }
}
