namespace FluxoCaixa.WebApi.Application;

/// <summary>
/// Repositório da aplicação para lançamentos
/// </summary>
public interface ILancamentoAppRepository
{
    /// <summary>
    /// Retorna o total de lançamentos baseado em um filtro
    /// </summary>
    /// <param name="filtro">Dados do filtro</param>
    /// <returns>Total de lançamentos</returns>
    Task<int> ObterTotalLancamentosPorFiltro(LancamentoFilter filtro);
}