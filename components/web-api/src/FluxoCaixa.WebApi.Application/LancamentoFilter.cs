using FluxoCaixa.WebApi.Application.RegistrarLancamento;

namespace FluxoCaixa.WebApi.Application;

/// <summary>
/// Filtro para pesquisa de lançamentos
/// </summary>
public record class LancamentoFilter
{
    public string? IdentificadorDono { get; init; }
    public TipoLancamento? Tipo { get; init; }
    public DateTimeOffset? DataHora { get; init; }
    public decimal? Valor { get; init; }
    public string? Descricao { get; init; }
}