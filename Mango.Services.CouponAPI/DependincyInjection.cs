using Microsoft.Extensions.DependencyInjection;

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


}
