using System;
using hands2hands.Services;
using hands2hands.Services.Interfaces;

namespace hands2hands.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<ICategoryService, CategoryService>();
        return services;
    }
}
