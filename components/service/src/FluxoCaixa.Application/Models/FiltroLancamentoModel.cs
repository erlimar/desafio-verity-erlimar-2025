namespace FluxoCaixa.Application.Models;

/// <summary>
/// Filtro para pesquisa de lançamentos
/// </summary>
public record class FiltroLancamentoModel
{
    public TipoLancamento? Tipo { get; init; }
    public DateTimeOffset? DataHora { get; init; }
    public decimal? Valor { get; init; }
    public string? Descricao { get; init; }
}