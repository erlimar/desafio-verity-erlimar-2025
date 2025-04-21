namespace FluxoCaixa.WebApi.Application.RegistrarLancamento;

/// <summary>
/// Executa o caso de uso "Registrar Lançamento"
/// </summary>
public class RegistrarLancamentoUseCase : IUseCaseWithInputForm<RegistrarLancamentoForm>
{
    public Task ExecAsync(RegistrarLancamentoForm form)
    {
        ValidateForm(form);

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
