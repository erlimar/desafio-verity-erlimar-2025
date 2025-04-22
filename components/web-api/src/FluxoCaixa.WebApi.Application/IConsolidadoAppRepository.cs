namespace FluxoCaixa.WebApi.Application;

/// <summary>
/// Repositório de aplicação para consolidado
/// </summary>
public interface IConsolidadoAppRepository
{
    /// <summary>
    /// Verifica se já existe um consolidado no dia informado
    /// </summary>
    /// <param name="date">Data para verificar</param>
    /// <returns><see cref="true"/>Se já existe</returns>
    Task<bool> ConsolidadoDoDiaExiste(DateOnly date);
}
