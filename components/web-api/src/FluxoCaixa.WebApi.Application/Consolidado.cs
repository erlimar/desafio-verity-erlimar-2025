namespace FluxoCaixa.WebApi.Application;

public record Consolidado
{
    public required string IdentificadorDono { get; init; }
    public required DateTimeOffset DataHora { get; init; }
    public required StatusConsolidado Status { get; init; }
}