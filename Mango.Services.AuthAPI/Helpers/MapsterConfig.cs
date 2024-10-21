using Mapster;
using System;
using System.Reflection;

namespace Mango.Services.AuthAPI.Helpers;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {

        //TypeAdapterConfig<ApplicationUser, UserResponse>
        //    .NewConfig()
        //    .Map();


        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}