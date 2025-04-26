using FluxoCaixa.Application.Models;

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
    Task<int> ContarLancamentosPorFiltroAsync(
        string identificadorDono,
        FiltroLancamentoModel filtro,
        CancellationToken cancellationToken);

    /// <summary>
    /// Grava dados de um lançamento
    /// </summary>
    /// <param name="lancamento">Dados do lançamento</param>
    Task GravarLancamentoAsync(
        LancamentoModel lancamento,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retorna uma lista de lançamentos no período à partir do offset informado
    /// </summary>
    /// <param name="identificadorDono">Identificador do dono</param>
    /// <param name="periodoInicial">Data inicial do período</param>
    /// <param name="periodoFinal">Data limite do período</param>
    /// <param name="offset">Posição no resultado à considerar para paginação</param>
    /// <returns></returns>
    Task<IEnumerable<LancamentoModel>> ListarLancamentosAsync(
        string identificadorDono,
        DateTimeOffset periodoInicial,
        DateTimeOffset periodoFinal,
        int offset,
        CancellationToken cancellationToken);

    /// <summary>
    /// Soma os valores dos lançamentos antes do período informado
    /// </summary>
    /// <param name="identificadorDono">Identificador do dono</param>
    /// <param name="dataLimite">Data limite para os lançamentos</param>
    /// <returns>Valor somado do período</returns>
    Task<decimal> SomarValoresLancamentosAntesDeAsync(
        string identificadorDono,
        DateTimeOffset dataLimite,
        CancellationToken cancellationToken);

    /// <summary>
    /// Soma os valores dos lançamentos no dia do período informado
    /// </summary>
    /// <param name="identificadorDono">Identificador do dono</param>
    /// <param name="dataLimite">Data para os lançamentos</param>
    /// <returns>Valor somado do período</returns>
    Task<decimal> SomarValoresLancamentosDoDiaAsync(
        string identificadorDono,
        DateTimeOffset dataLimite,
        CancellationToken cancellationToken);
}