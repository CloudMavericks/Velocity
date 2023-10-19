using Velocity.Backend.Configurations;

namespace Velocity.Backend.Extensions;

internal static class ConfigurationExtensions
{
    public static IServiceCollection AddTokenConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TokenConfiguration>(configuration.GetSection(nameof(TokenConfiguration)));
        return services;
    }
    
    public static TokenConfiguration GetTokenConfiguration(this IConfiguration configuration)
    {
        return configuration.GetSection(nameof(TokenConfiguration)).Get<TokenConfiguration>();
    }
}