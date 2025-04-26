namespace FluxoCaixa.Application.CalcularConsolidacao;

/// <summary>
/// Dados de entrada para caso de uso <see cref="CalcularConsolidacaoUseCase"/>
/// </summary>
/// <param name="ConsolidadoId">Identificador do consolidado</param>
public record CalcularConsolidacaoForm(string ConsolidadoId);