namespace FluxoCaixa.Application;

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
    Task<int> ObterTotalLancamentosPorFiltroAsync(
        LancamentoFilter filtro,
        CancellationToken cancellationToken);

    /// <summary>
    /// Grava dados de um lançamento
    /// </summary>
    /// <param name="lancamento">Dados do lançamento</param>
    Task GravarLancamentoAsync(Lancamento lancamento, CancellationToken cancellationToken);

    /// <summary>
    /// Retorna uma lista de lançamentos no período à partir do offset informado
    /// </summary>
    /// <param name="identificadorDono">Identificador do dono</param>
    /// <param name="periodoInicial">Data inicial do período</param>
    /// <param name="periodoFinal">Data limite do período</param>
    /// <param name="offset">Posição no resultado à considerar para paginação</param>
    /// <returns></returns>
    Task<IEnumerable<Lancamento>> ListarLancamentosAsync(
        string identificadorDono,
        DateTimeOffset periodoInicial,
        DateTimeOffset periodoFinal,
        int offset = 0);
}