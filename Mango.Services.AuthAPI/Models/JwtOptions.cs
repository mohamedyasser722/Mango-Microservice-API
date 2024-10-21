using System.ComponentModel.DataAnnotations;

namespace Mango.Services.AuthAPI.Models;

public class JwtOptions
{
    public static string SectionName = "ApiSettings:JwtOptions";


    [Required]
    public string Secret { get; set; } = string.Empty;
    [Required]
    public double TokenLifetime { get; set; }
    [Required]
    public string Issuer { get; set; } = string.Empty;
    [Required]
    public string Audience { get; set; } = string.Empty;
}
