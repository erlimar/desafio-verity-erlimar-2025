namespace FluxoCaixa.Application.BuscarConsolidadoDiario;

/// <summary>
/// Executa o caso de uso "Visualizar consolidação diária", que obtém os dados
/// de uma consolidação em um dia específico
/// </summary>
public class BuscarConsolidadoDiarioUseCase : IUseCaseInputOutput<BuscarConsolidadoDiarioForm, Consolidado?>
{
    private readonly IConsolidadoAppRepository _consolidadoAppRepository;

    public BuscarConsolidadoDiarioUseCase(IConsolidadoAppRepository consolidadoAppRepository)
    {
        _consolidadoAppRepository = consolidadoAppRepository ??
            throw new ArgumentNullException(nameof(consolidadoAppRepository));
    }

    /// <summary>
    /// Executa o caso de uso
    /// </summary>
    /// <param name="form">Dados de entrada</param>
    /// <returns>Retorna instância de <see cref="Consolidado"/> ou nulo</returns>
    /// <exception cref="ArgumentNullException">
    /// Quando os dados de entrada são inválidos
    /// </exception>
    public async Task<Consolidado?> ExecAsync(
        BuscarConsolidadoDiarioForm form,
        CancellationToken cancellationToken)
    {
        ValidateForm(form);

        form.Data.Deconstruct(out DateOnly dataOriginal, out _, out TimeSpan timeSpanOriginal);

        var dataApenas = new DateTimeOffset(dataOriginal, new TimeOnly(0), timeSpanOriginal);

        var resultado = await _consolidadoAppRepository.ObterConsolidadosDaDataAsync(
            form.IdentificadorDono,
            dataApenas,
            cancellationToken);

        return resultado?.FirstOrDefault();
    }

    private static void ValidateForm(BuscarConsolidadoDiarioForm form)
    {
        _ = form ?? throw new ArgumentNullException(nameof(form));

        if (string.IsNullOrWhiteSpace(form.IdentificadorDono))
        {
            throw new ArgumentException(
                "O identificador do dono não pode ser vazio",
                nameof(form.IdentificadorDono));
        }
    }
}