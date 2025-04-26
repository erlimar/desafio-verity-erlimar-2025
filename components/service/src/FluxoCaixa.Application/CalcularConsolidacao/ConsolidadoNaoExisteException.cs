namespace FluxoCaixa.Application.CalcularConsolidacao;

/// <summary>
/// Quando tenta-se calcular o consolidado sem registro
/// </summary>
public class ConsolidadoNaoExisteException(string consolidadoId)
    : Exception($"""O consolidado de identificador "{consolidadoId}" não existe""")
{ }