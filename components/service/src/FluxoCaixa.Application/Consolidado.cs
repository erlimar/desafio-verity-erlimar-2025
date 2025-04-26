namespace FluxoCaixa.Application;

public record Consolidado
{
    public string? Id { get; init; }
    public required string IdentificadorDono { get; init; }
    public required DateTimeOffset DataHora { get; init; }
    public required StatusConsolidado Status { get; init; }
    public decimal SaldoAnterior { get; init; }
    public decimal SaldoPeriodo { get; init; }
    public decimal SaldoFinal { get; init; }
}