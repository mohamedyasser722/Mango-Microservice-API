namespace Mango.Services.AuthAPI.Services.IServices;

public interface IJwtTokenGenerator
{
    (string token, double expiresIn) GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    string? ValidateToken(string token);
    RefreshToken GenerateRefreshToken();
}
