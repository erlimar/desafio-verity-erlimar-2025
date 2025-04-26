using FluxoCaixa.Application.Models;

namespace FluxoCaixa.Application;

/// <summary>
/// Repositório de aplicação para consolidado
/// </summary>
public interface IConsolidadoAppRepository
{
    /// <summary>
    /// Obtem uma lista de consolidados registrados à partir do dia informado
    /// </summary>
    /// <param name="identificadorDono">Identificador do dono</param>
    /// <param name="dataHora">Data/hora para verificar</param>
    /// <returns>Lista de consolidados registrados, ou lista vazia caso não existam</returns>
    Task<IEnumerable<ConsolidadoModel>> ObterConsolidadosAPartirDoDiaAsync(
        string identificadorDono,
        DateTimeOffset dataHora,
        CancellationToken cancellationToken);

    /// <summary>
    /// Obtem uma lista de consolidados registrados na data informada
    /// </summary>
    /// <param name="identificadorDono">Identificador do dono</param>
    /// <param name="dataHora">Data/hora para verificar</param>
    /// <returns>Lista de consolidados registrados, ou lista vazia caso não existam</returns>
    Task<IEnumerable<ConsolidadoModel>> ObterConsolidadosDaDataAsync(
        string identificadorDono,
        DateTimeOffset dataHora,
        CancellationToken cancellationToken);

    /// <summary>
    /// Grava dados de um consolidado. Substituindo caso já exista
    /// </summary>
    /// <param name="dados">Dados para gravação</param>
    Task GravarConsolidadoAsync(ConsolidadoModel dados, CancellationToken cancellationToken);

    /// <summary>
    /// Obtém um consolidado por identificador
    /// </summary>
    /// <param name="consolidadoId">Identificador do consolidado</param>
    /// <returns>Instância de <see cref="ConsolidadoModel"/> ou nulo se não existir</returns>
    Task<ConsolidadoModel?> ObterPorIdAsync(string consolidadoId, CancellationToken cancellationToken);
}