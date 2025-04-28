using FluxoCaixa.Application.RegistrarLancamento;

namespace FluxoCaixa.WebApi;

/// <summary>
/// Métodos para configurar recursos da camada de aplicação
/// </summary>
public static class ApplicationExtensions
{
    /// <summary>
    /// Adiciona suporte aos casos de uso da aplicação
    /// </summary>
    public static void AddApplicationUseCases(this IServiceCollection services)
    {
        services.AddTransient<RegistrarLancamentoUseCase>();
    }
}