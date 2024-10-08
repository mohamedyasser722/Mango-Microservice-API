using FluentValidation;
using Mango.Services.CouponAPI.Contracts;
using Mango.Services.CouponAPI.Services;
using Mapster;

namespace Mango.Services.CouponAPI;

public static class DependincyInjection
{

    public static void RegisterAppServices(this IServiceCollection services)
    {
        services.RegisterMapsterConfiguration();
        services.AddValidatorsFromAssemblyContaining<RefrenceValidatorAssembly>();
        services.AddScoped<ICouponService, CouponService>();

    }

}
