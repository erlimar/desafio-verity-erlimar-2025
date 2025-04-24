namespace FluxoCaixa.WebApi.Application.RegistrarLancamento;

public record ConsolidarMessage
{
    public required DateTimeOffset Data { get; init; }
}