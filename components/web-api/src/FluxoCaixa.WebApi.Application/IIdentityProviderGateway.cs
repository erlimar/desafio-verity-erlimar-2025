namespace FluxoCaixa.WebApi.Application.RegistrarLancamento;

/// <summary>
/// Interface com provedor de identidade
/// </summary>
public interface IIdentityProviderGateway
{
    Task<bool> UsuarioExisteAsync(string id, CancellationToken cancellationToken);
}