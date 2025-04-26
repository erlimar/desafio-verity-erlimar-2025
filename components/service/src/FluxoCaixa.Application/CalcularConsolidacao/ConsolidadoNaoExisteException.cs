namespace FluxoCaixa.Application.CalcularConsolidacao;

/// <summary>
/// Quando tenta-se calcular o consolidado sem registro
/// </summary>
public class ConsolidadoNaoExisteException : Exception
{
    public ConsolidadoNaoExisteException(string consolidadoId)
        : base($"""O consolidado de identificador "{consolidadoId}" não existe""")
    {
    }
}