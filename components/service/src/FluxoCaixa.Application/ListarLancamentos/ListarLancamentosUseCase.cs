namespace FluxoCaixa.Application.ListarLancamentos;

/// <summary>
/// Executa o caso de uso "Visualizar lançamentos", que pesquisa os lançamentos
/// em uma janela de período de forma paginada
/// </summary>
public class ListarLancamentosUseCase : IUseCaseInputOutput<ListarLancamentosForm, IEnumerable<Lancamento>>
{
    private readonly ILancamentoAppRepository _lancamentoAppRepository;

    public ListarLancamentosUseCase(ILancamentoAppRepository lancamentoAppRepository)
    {
        _lancamentoAppRepository = lancamentoAppRepository ??
            throw new ArgumentNullException(nameof(lancamentoAppRepository));
    }

    /// <summary>
    /// Executa o caso de uso
    /// </summary>
    /// <param name="form">Dados de entrada</param>
    /// <returns>Retorna lançamentos encontrados</returns>
    /// <exception cref="ArgumentNullException">
    /// Quando os dados de entrada são inválidos
    /// </exception>
    /// <exception cref="PesquisaPeriodoMaiorQue90DiasException">
    /// Quando o período da pesquisa for superior a 90 dias
    /// </exception>
    public async Task<IEnumerable<Lancamento>> ExecAsync(
        ListarLancamentosForm form,
        CancellationToken cancellationToken)
    {
        ValidateForm(form);

        TimeSpan diferenca = form.PeriodoFinal.Subtract(form.PeriodoInicial);

        // Regra: A pesquisa deve ser feita obrigatoriamente entre um período
        //        determinado de até 90 dias
        if (diferenca.Days > 90)
        {
            throw new PesquisaPeriodoMaiorQue90DiasException();
        }

        return await _lancamentoAppRepository.ListarLancamentosAsync(
            form.IdentificadorDono,
            form.PeriodoInicial,
            form.PeriodoFinal,
            form.Offset,
            cancellationToken);
    }

    private static void ValidateForm(ListarLancamentosForm form)
    {
        _ = form ?? throw new ArgumentNullException(nameof(form));

        if (string.IsNullOrWhiteSpace(form.IdentificadorDono))
        {
            throw new ArgumentException(
                "O identificador do dono não pode ser vazio",
                nameof(form.IdentificadorDono));
        }

        if (DateOnly.FromDateTime(form.PeriodoInicial.DateTime)
            > DateOnly.FromDateTime(form.PeriodoFinal.DateTime))
        {
            throw new ArgumentException(
                "Você deve informar um período final maior ou igual ao período inicial",
                nameof(form.PeriodoFinal));
        }

        if (form.Offset < 0)
        {
            throw new ArgumentException(
                "Informe um Offset à partir de zero",
                nameof(form.Offset));
        }
    }
}