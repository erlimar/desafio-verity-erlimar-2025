using FluxoCaixa.Application.RegistrarLancamento;

namespace FluxoCaixa.KeycloakGateway;

/// <summary>
/// Gateway de identidade do usuário para Keycloak
/// </summary>
public class KeycloakUserIdentityGateway : IUserIdentityGateway
{
    public Task<bool> UsuarioExisteAsync(string id, CancellationToken cancellationToken)
    {
        // NOTE: Devido ao tempo para implementação, este "gateway" não está implementado.
        //       Ao invés disso sempre retorna que o usuário existe. Mas essa implementação
        //       deve acessar o serviço de identidade para validar o código do usuário.
        return Task.FromResult(true);
    }
}