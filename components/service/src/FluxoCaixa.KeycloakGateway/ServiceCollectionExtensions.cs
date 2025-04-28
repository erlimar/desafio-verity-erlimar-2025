using FluxoCaixa.Application.RegistrarLancamento;

using Microsoft.Extensions.DependencyInjection;

namespace FluxoCaixa.KeycloakGateway;

/// <summary>
/// Extensões para <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adiciona suporte Keycloak para <see cref="IUserIdentityGateway"/>
    /// </summary>
    public static void AddKeycloakUserIdentityGateway(this IServiceCollection services)
    {
        services.AddTransient<IUserIdentityGateway, KeycloakUserIdentityGateway>();
    }
}