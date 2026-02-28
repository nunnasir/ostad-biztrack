using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using biztrack.ostad.Application.Interfaces;
using biztrack.ostad.Application.Implementations;
using biztrack.ostad.Infrastructure;

namespace biztrack.ostad.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddBizTrackApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBizTrackInfrastructure(configuration);
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        return services;
    }
}
