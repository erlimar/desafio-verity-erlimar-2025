namespace FluxoCaixa.WebApi.Application.RegistrarLancamento;

/// <summary>
/// Executa o caso de uso "Registrar Lançamento"
/// </summary>
public class RegistrarLancamentoUseCase : IUseCaseWithInputForm<RegistrarLancamentoForm>
{
    private readonly IIdentityProviderGateway _identityProviderGateway;

    public RegistrarLancamentoUseCase(IIdentityProviderGateway identityProviderGateway)
    {
        _identityProviderGateway = identityProviderGateway
            ?? throw new ArgumentNullException(nameof(identityProviderGateway));
    }

    /// <summary>
    /// Executa o caso de uso
    /// </summary>
    /// <param name="form">Dados de entrada</param>
    /// <returns></returns>
    /// <exception cref="DonoLancamentoInvalidoException">Quando o dono do lançamento não existe</exception>
    public Task ExecAsync(RegistrarLancamentoForm form)
    {
        ValidateForm(form);

        if (!_identityProviderGateway.UsuarioExiste(form.IdentificadorDono))
        {
            throw new DonoLancamentoInvalidoException(form.IdentificadorDono);
        }

        throw new NotImplementedException();
    }

    private static void ValidateForm(RegistrarLancamentoForm form)
    {
        _ = form ?? throw new ArgumentNullException(nameof(form));

        if (string.IsNullOrWhiteSpace(form.IdentificadorDono))
        {
            throw new ArgumentException("O identificador do dono não pode ser vazio", nameof(form.IdentificadorDono));
        }

        if (string.IsNullOrWhiteSpace(form.Descricao))
        {
            throw new ArgumentException("A descrição não pode ser vazia", nameof(form.Descricao));
        }

        if (form.Valor <= 0m)
        {
            throw new ArgumentException("O valor precisa ser maior que zero", nameof(form.Valor));
        }
    }
}