using FluxoCaixa.Application;
using FluxoCaixa.Application.Models;

namespace FluxoCaixa.DatabaseAccess;

public class MongoDBLancamentoAppRepository : ILancamentoAppRepository
{
    public Task<int> ContarLancamentosPorFiltroAsync(
        string identificadorDono,
        FiltroLancamentoModel filtro,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task GravarLancamentoAsync(
        LancamentoModel lancamento,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LancamentoModel>> ListarLancamentosAsync(
        string identificadorDono,
        DateTimeOffset periodoInicial,
        DateTimeOffset periodoFinal,
        int offset,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> SomarValoresLancamentosAntesDeAsync(
        string identificadorDono,
        DateTimeOffset dataLimite,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> SomarValoresLancamentosDoDiaAsync(
        string identificadorDono,
        DateTimeOffset dataLimite,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}