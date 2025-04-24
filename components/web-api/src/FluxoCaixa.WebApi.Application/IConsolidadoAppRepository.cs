namespace FluxoCaixa.WebApi.Application;

/// <summary>
/// Repositório de aplicação para consolidado
/// </summary>
public interface IConsolidadoAppRepository
{
    /// <summary>
    /// Verifica se já existe um consolidado no dia informado
    /// </summary>
    /// <param name="dataHora">Data/hora para verificar</param>
    /// <returns><see cref="true"/>Se já existe</returns>
    Task<bool> ExisteConsolidadoDoDiaAsync(DateTimeOffset dataHora);

    /// <summary>
    /// Grava dados de um consolidado. Substituindo caso já exista
    /// </summary>
    /// <param name="dados">Dados para gravação</param>
    Task GravarConsolidadoAsync(Consolidado dados);
}