namespace FluxoCaixa.WebApi.Application.RegistrarLancamento;

/// <summary>
/// Interface com provedor de identidade
/// </summary>
public interface IIdentityProviderGateway
{
    bool UsuarioExiste(string id);
}