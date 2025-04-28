using FluxoCaixa.Application;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

namespace FluxoCaixa.DatabaseAccess;

/// <summary>
/// Extensões para <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adiciona suporte ao cliente do MongoDB
    /// </summary>
    /// <param name="connectionStringName">Nome da string de conexão</param>
    public static void AddMongoClient(
        this IServiceCollection services,
        string connectionStringName,
        string databaseName)
    {
        if (string.IsNullOrWhiteSpace(connectionStringName))
        {
            throw new ArgumentNullException(nameof(connectionStringName));
        }

        if (string.IsNullOrWhiteSpace(databaseName))
        {
            throw new ArgumentNullException(nameof(databaseName));
        }

        services.AddTransient(sp =>
        {
            var connectionString = sp.GetRequiredService<IConfiguration>()
                .GetConnectionString(connectionStringName);

            var client = string.IsNullOrWhiteSpace(connectionString)
                ? throw new InvalidOperationException(
                    $"""A string de conexão "{connectionStringName}" não foi configurada.""")
                : new MongoClient(connectionString);

            return client.GetDatabase(databaseName);
        });
    }

    /// <summary>
    /// Adiciona suporte a repositórios MongoDB
    /// </summary>
    public static void AddMongoDBRepositories(this IServiceCollection services)
    {
        services.AddTransient<IConsolidadoAppRepository, MongoDBConsolidadoAppRepository>();
        services.AddTransient<ILancamentoAppRepository, MongoDBLancamentoAppRepository>();
    }
}