namespace Mango.Services.AuthAPI.Models;

public class RefreshToken
{
    public int Id { get; set; } // Primary key
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => Revoked.HasValue;
    public bool IsActive => !IsExpired && !IsRevoked ;

}
