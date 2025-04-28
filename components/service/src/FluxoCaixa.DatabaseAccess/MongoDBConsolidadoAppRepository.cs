using FluxoCaixa.Application;
using FluxoCaixa.Application.Models;

namespace FluxoCaixa.DatabaseAccess;

public class MongoDBConsolidadoAppRepository : IConsolidadoAppRepository
{
    public Task GravarConsolidadoAsync(
        ConsolidadoModel dados,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ConsolidadoModel>> ObterConsolidadosAPartirDoDiaAsync(
        string identificadorDono,
        DateTimeOffset dataHora,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ConsolidadoModel>> ObterConsolidadosDaDataAsync(
        string identificadorDono,
        DateTimeOffset dataHora,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ConsolidadoModel?> ObterPorIdAsync(
        string consolidadoId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}