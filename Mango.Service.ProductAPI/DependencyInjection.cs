using Mango.Service.ProductAPI.Data;
using Mango.Service.ProductAPI.Services;
using Mango.Service.ProductAPI.Services.IServices;
using Mango.Services.ProductAPI.Helpers;
using Microsoft.EntityFrameworkCore;

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
}
