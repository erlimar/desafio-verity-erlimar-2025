namespace FluxoCaixa.Application.RegistrarLancamento;

public record ConsolidarMessage
{
    public required DateTimeOffset Data { get; init; }
}