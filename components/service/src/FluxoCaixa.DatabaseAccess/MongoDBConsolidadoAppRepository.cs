using FluxoCaixa.Application;
using FluxoCaixa.Application.Models;

using MongoDB.Driver;

namespace FluxoCaixa.DatabaseAccess;

public class MongoDBConsolidadoAppRepository(IMongoDatabase database)
    : IConsolidadoAppRepository
{
    private readonly IMongoDatabase _db = database
        ?? throw new ArgumentNullException(nameof(database));

    private IMongoCollection<ConsolidadoModel> GetConsolidadoCollection()
        => _db.GetCollection<ConsolidadoModel>("consolidados");

    public Task GravarConsolidadoAsync(
        ConsolidadoModel dados,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ConsolidadoModel>> ObterConsolidadosAPartirDoDiaAsync(
        string identificadorDono,
        DateTimeOffset dataHora,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(identificadorDono))
        {
            throw new ArgumentNullException(nameof(identificadorDono));
        }

        var filtroMongo = Builders<ConsolidadoModel>.Filter.And(
            Builders<ConsolidadoModel>.Filter.Eq(f => f.IdentificadorDono, identificadorDono),
            Builders<ConsolidadoModel>.Filter.Gte(f => f.DataHora, dataHora));

        var documents = await GetConsolidadoCollection().FindAsync(
            filtroMongo,
            cancellationToken: cancellationToken);

        var docs = await documents.ToListAsync();

        return docs;
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