namespace FluxoCaixa.WebApi.Application;

/// <summary>
/// Um lançamento registrado
/// </summary>
public record class Lancamento
{
    public required string IdentificadorDono { get; init; }
    public required TipoLancamento Tipo { get; init; }
    public required DateTimeOffset DataHora { get; init; }
    public required decimal Valor { get; init; }
    public required string Descricao { get; init; }
}