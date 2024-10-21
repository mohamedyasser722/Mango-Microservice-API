using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Mango.Services.CouponAPI;

public static class DependincyInjection
{

    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services.RegisterMapsterConfiguration();
        services.AddValidatorsFromAssemblyContaining<RefrenceValidatorAssembly>();

        services.AddScoped<CouponService>();
        services.AddScoped<ICacheService, InMemoryCacheService>();
        //services.AddScoped<ICouponService>(provider =>
        //{
        //    var coupounService = provider.GetRequiredService<CouponService>();

        //    return new DecoratorCachedCouponService(coupounService, provider.GetService<IMemoryCache>()!, provider.GetService<ICacheService>()!,
        //        provider.GetService<ILogger<DecoratorCachedCouponService>>());

        //});
        services.AddScoped<ICouponService, InMemoryCacheCouponService>();
        services.AddMemoryCache();

        return services;
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
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
            };
        });

        return builder;
    }


}
