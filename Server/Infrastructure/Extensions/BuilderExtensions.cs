using Domain;
using Infrastructure.DataBase;
using Infrastructure.Services.HashService;
using Infrastructure.Services.JwtService;
using Infrastructure.Services.UrlShortenerService;
using Infrastructure.Services.ValidationService;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.Extensions;

public static class BuilderExtensions
{
    public static void ConfigureInfrastructureLayer(this WebApplicationBuilder builder)
    {
        builder.Services.AddJwtService();
        builder.Services.AddHashService();
        builder.Services.AddUnitOfWork();
        builder.Services.AddUrlShortenerService();
        builder.Services.AddValidationService();
    }

    private static void AddJwtService(this IServiceCollection services)
    {
        services.AddSingleton<IJwtService, JwtService>();
    }

    private static void AddHashService(this IServiceCollection services)
    {
        services.AddSingleton<IHashService, HashService>();
    }
    private static void AddValidationService(this IServiceCollection services)
    {
        services.AddSingleton<IValidationService, ValidationService>();
    }

    private static void AddUrlShortenerService(this IServiceCollection services)
    {
        services.AddSingleton<IUrlShortenerService, UrlShortenerService>();
    }

    private static void AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
