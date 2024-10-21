using Mango.Service.ProductAPI.Data;
using Mango.Service.ProductAPI.Services;
using Mango.Service.ProductAPI.Services.IServices;
using Mango.Services.ProductAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Mango.Service.ProductAPI;

public static class DependencyInjection
{
    public static void AddProductAPIServices(this WebApplicationBuilder builder)
    {
        // Add Database
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        // Register Mapster
        builder.Services.RegisterMapsterConfiguration();

        // Register Services
        builder.Services.AddScoped<IProductService, ProductService>();



    }

    public static WebApplicationBuilder RegisterJwtConfiguration(this WebApplicationBuilder builder)
    {
        var settingsSection = builder.Configuration.GetSection("ApiSettings");

        var secret = settingsSection.GetValue<string>("Secret");
        var issuer = settingsSection.GetValue<string>("Issuer");
        var audience = settingsSection.GetValue<string>("Audience");

        var key = Encoding.ASCII.GetBytes(secret);

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(b =>
        {
            b.TokenValidationParameters = new TokenValidationParameters
            {
                // Validate that the token has a valid signing key
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                // Validate the issuer of the token (to prevent misuse across different apps)
                ValidateIssuer = true,
                ValidIssuer = issuer,

                // Validate the audience of the token (who the token is intended for)
                ValidateAudience = true,
                ValidAudience = audience,

                // Enable token expiration validation
                ValidateLifetime = true, // <-- This checks for token expiration and it is enabled by default
            };
        });


        return builder;
    }
}
