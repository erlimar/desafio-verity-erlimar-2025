namespace FluxoCaixa.WebApi.Utils;

/// <summary>
/// Opções de configuração de CORS
/// </summary>
public class CorsConfigurationOptions
{
    /// <summary>
    /// Lista de origens permitidas para CORS
    /// </summary>
    public string? AllowedOrigins { get; set; }
}