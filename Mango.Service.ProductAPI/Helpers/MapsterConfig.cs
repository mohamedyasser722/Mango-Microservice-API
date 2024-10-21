using Mango.Service.ProductAPI.Contracts.Product;
using Mapster;
using System;
using System.Reflection;

namespace Mango.Services.ProductAPI.Helpers;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {

        TypeAdapterConfig<ProductRequest, ProductResponse>
            .NewConfig();


        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}