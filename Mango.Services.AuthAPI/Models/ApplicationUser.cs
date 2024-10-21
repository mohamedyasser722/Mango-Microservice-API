namespace Mango.Services.AuthAPI.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = [];
}
