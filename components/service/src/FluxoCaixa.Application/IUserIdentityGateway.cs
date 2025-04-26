namespace FluxoCaixa.Application.RegistrarLancamento;

/// <summary>
/// Adaptador de comunicação com provedor de identidade do usuário
/// </summary>
public interface IUserIdentityGateway
{
    Task<bool> UsuarioExisteAsync(string id, CancellationToken cancellationToken);
}