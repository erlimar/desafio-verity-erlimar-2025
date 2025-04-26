namespace FluxoCaixa.Application;

/// <summary>
/// Filtro para pesquisa de lançamentos
/// </summary>
public record class LancamentoFilter
{
    public TipoLancamento? Tipo { get; init; }
    public DateTimeOffset? DataHora { get; init; }
    public decimal? Valor { get; init; }
    public string? Descricao { get; init; }
}