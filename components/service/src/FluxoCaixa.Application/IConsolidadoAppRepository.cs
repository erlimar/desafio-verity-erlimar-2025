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
    Task<IEnumerable<Consolidado>> ObterConsolidadosAPartirDoDiaAsync(
        string identificadorDono,
        DateTimeOffset dataHora,
        CancellationToken cancellationToken);

    /// <summary>
    /// Grava dados de um consolidado. Substituindo caso já exista
    /// </summary>
    /// <param name="dados">Dados para gravação</param>
    Task GravarConsolidadoAsync(Consolidado dados, CancellationToken cancellationToken);
}