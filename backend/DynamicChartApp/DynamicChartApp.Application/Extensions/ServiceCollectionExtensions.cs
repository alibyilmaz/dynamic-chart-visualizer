using DynamicChartApp.Application.Services;
using DynamicChartApp.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicChartApp.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Application Services
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<ILoggingService, LoggingService>();
        services.AddScoped<IErrorHandlingService, ErrorHandlingService>();
        services.AddScoped<DataService>();

        // Infrastructure Services
        services.AddScoped<IConnectionService, ConnectionService>();

        return services;
    }
}
