using FluxoCaixa.Application;
using FluxoCaixa.Application.Models;

using MongoDB.Bson;
using MongoDB.Driver;

namespace FluxoCaixa.DatabaseAccess;

public class MongoDBLancamentoAppRepository(IMongoDatabase database)
    : ILancamentoAppRepository
{
    private readonly IMongoDatabase _db = database
        ?? throw new ArgumentNullException(nameof(database));

    private IMongoCollection<LancamentoModel> GetLancamentoCollection()
        => _db.GetCollection<LancamentoModel>("lancamentos");

    public async Task<long> ContarLancamentosPorFiltroAsync(
        string identificadorDono,
        FiltroLancamentoModel filtro,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(identificadorDono))
        {
            throw new ArgumentNullException(nameof(identificadorDono));
        }

        _ = filtro ?? throw new ArgumentNullException(nameof(filtro));

        var filtroMongo = Builders<LancamentoModel>.Filter.Eq(
            f => f.IdentificadorDono, identificadorDono);

        if (filtro.Tipo is not null)
        {
            filtroMongo &= Builders<LancamentoModel>.Filter.Eq(
                f => f.Tipo, filtro.Tipo);
        }

        if (filtro.DataHora is not null)
        {
            filtroMongo &= Builders<LancamentoModel>.Filter.Eq(
                f => f.DataHora, filtro.DataHora);
        }

        if (filtro.Descricao is not null)
        {
            filtroMongo &= Builders<LancamentoModel>.Filter.Eq(
                f => f.Descricao, filtro.Descricao);
        }

        var collection = GetLancamentoCollection();

        var documents = await collection.FindAsync(
            filtroMongo,
            cancellationToken: cancellationToken);

        var docs = await documents.ToListAsync();

        var count = await collection.CountDocumentsAsync(
            filtroMongo,
            cancellationToken: cancellationToken);

        return count;
    }

    public async Task GravarLancamentoAsync(
        LancamentoModel lancamento,
        CancellationToken cancellationToken)
    {
        _ = lancamento ?? throw new ArgumentNullException(nameof(lancamento));

        await GetLancamentoCollection().InsertOneAsync(
            new()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                IdentificadorDono = lancamento.IdentificadorDono,
                Tipo = lancamento.Tipo,
                DataHora = lancamento.DataHora,
                Valor = lancamento.Valor,
                Descricao = lancamento.Descricao
            },
            cancellationToken: cancellationToken);
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