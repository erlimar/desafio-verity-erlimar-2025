
namespace FluxoCaixa.Application.CalcularConsolidacao;

/// <summary>
/// Executa o caso de uso "Calcular consolidação diária", que faz os cálculos e
/// registra esses dados para consulta de saldo consolidado em um determinado
/// dia
/// </summary>
public class CalcularConsolidacaoUseCase : IUseCaseInputOutput<CalcularConsolidacaoForm, Consolidado>
{
    private readonly ILancamentoAppRepository _lancamentoAppRepository;
    private readonly IConsolidadoAppRepository _consolidadoAppRepository;

    public CalcularConsolidacaoUseCase(
        ILancamentoAppRepository lancamentoAppRepository,
        IConsolidadoAppRepository consolidadoAppRepository)
    {
        _lancamentoAppRepository = lancamentoAppRepository
            ?? throw new ArgumentNullException(nameof(lancamentoAppRepository));

        _consolidadoAppRepository = consolidadoAppRepository
            ?? throw new ArgumentNullException(nameof(consolidadoAppRepository));
    }

    /// <summary>
    /// Executa o caso de uso
    /// </summary>
    /// <param name="form">Dados de entrada</param>
    /// <returns>Instância de <see cref="Consolidado"/></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<Consolidado> ExecAsync(
        CalcularConsolidacaoForm form,
        CancellationToken cancellationToken)
    {
        ValidateForm(form);

        var consolidadoOriginal = await _consolidadoAppRepository.ObterPorIdAsync(
            form.ConsolidadoId,
            cancellationToken)
            ?? throw new ConsolidadoNaoExisteException(form.ConsolidadoId);

        consolidadoOriginal.DataHora.Deconstruct(
            out DateOnly dataOriginal,
            out _,
            out TimeSpan timeSpanOriginal);

        var dataDoDia = new DateTimeOffset(
            dataOriginal,
            new TimeOnly(0, 0, 0, 0, 0),
            timeSpanOriginal);

        var saldoAnterior = await _lancamentoAppRepository.SomarValoresLancamentosAntesDeAsync(
            consolidadoOriginal.IdentificadorDono,
            dataDoDia,
            cancellationToken);

        var saldoDia = await _lancamentoAppRepository.SomarValoresLancamentosDoDiaAsync(
            consolidadoOriginal.IdentificadorDono,
            dataDoDia,
            cancellationToken);

        var consolidadoAtualizado = new Consolidado()
        {
            Id = consolidadoOriginal.Id,
            IdentificadorDono = consolidadoOriginal.IdentificadorDono,
            DataHora = consolidadoOriginal.DataHora,
            Status = consolidadoOriginal.Status,
            SaldoAnterior = consolidadoOriginal.SaldoAnterior,
            SaldoPeriodo = consolidadoOriginal.SaldoPeriodo,
            SaldoFinal = saldoAnterior + saldoDia,
        };

        await _consolidadoAppRepository.GravarConsolidadoAsync(
            consolidadoAtualizado,
            cancellationToken);

        return consolidadoAtualizado;
    }

    private static void ValidateForm(CalcularConsolidacaoForm form)
    {
        _ = form ?? throw new ArgumentNullException(nameof(form));

        if (string.IsNullOrWhiteSpace(form.ConsolidadoId))
        {
            throw new ArgumentException(
                "O identificador do consolidado não pode ser vazio",
                nameof(form.ConsolidadoId));
        }
    }
}