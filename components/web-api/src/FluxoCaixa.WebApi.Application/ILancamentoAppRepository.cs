namespace FluxoCaixa.WebApi.Application;

/// <summary>
/// Repositório de aplicação para lançamentos
/// </summary>
public interface ILancamentoAppRepository
{
    /// <summary>
    /// Retorna o total de lançamentos baseado em um filtro
    /// </summary>
    /// <param name="filtro">Dados do filtro</param>
    /// <returns>Total de lançamentos</returns>
    Task<int> ObterTotalLancamentosPorFiltroAsync(LancamentoFilter filtro, CancellationToken cancellationToken);

    /// <summary>
    /// Grava dados de um lançamento
    /// </summary>
    /// <param name="lancamento">Dados do lançamento</param>
    Task GravarLancamentoAsync(Lancamento lancamento, CancellationToken cancellationToken);
}