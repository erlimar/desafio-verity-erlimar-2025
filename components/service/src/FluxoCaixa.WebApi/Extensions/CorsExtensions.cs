using FluxoCaixa.WebApi.Utils;

namespace FluxoCaixa.WebApi.Extensions;

/// <summary>
/// Métodos para configurar CORS
/// </summary>
public static class CorsExtensions
{
    /// <summary>
    /// Adiciona suporte a CORS
    /// </summary>
    public static void AddApplicationCors(this IServiceCollection services, IConfiguration configuration)
    {
        var corsConfigOptions = configuration.GetSection("Cors").Get<CorsConfigurationOptions>();

        services.AddCors(
            options => options.AddDefaultPolicy(
                policy => policy
                .WithOrigins(corsConfigOptions?.AllowedOrigins!)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()));
    }
}
