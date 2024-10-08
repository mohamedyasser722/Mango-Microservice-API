using Mango.Services.CouponAPI.Contracts.Coupon;
using Mango.Services.CouponAPI.Models;
using Mapster;
using System;
using System.Reflection;

namespace Mango.Services.CouponAPI;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<Coupon, CouponResponse>
        .NewConfig(); 
        TypeAdapterConfig<Coupon, CouponRequest>
        .NewConfig();




        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}