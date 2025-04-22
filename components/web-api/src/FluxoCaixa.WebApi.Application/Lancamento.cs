using FluxoCaixa.WebApi.Application.RegistrarLancamento;

namespace FluxoCaixa.WebApi.Application;

/// <summary>
/// Um lançamento registrado
/// </summary>
public record class Lancamento
{
    public required string Id { get; init; }
    public required string IdentificadorDono { get; init; }
    public TipoLancamento Tipo { get; init; }
    public DateTimeOffset DataHora { get; init; }
    public decimal Valor { get; init; }
    public required string Descricao { get; init; }
}