using FluxoCaixa.Application;

using Microsoft.Extensions.DependencyInjection;

namespace FluxoCaixa.DatabaseAccess;

/// <summary>
/// Extensões para <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adiciona suporte a repositórios MongoDB
    /// </summary>
    public static void AddMongoDBRepositories(this IServiceCollection services)
    {
        services.AddTransient<IConsolidadoAppRepository, MongoDBConsolidadoAppRepository>();
        services.AddTransient<ILancamentoAppRepository, MongoDBLancamentoAppRepository>();
    }
}