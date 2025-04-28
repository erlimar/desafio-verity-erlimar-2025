using FluxoCaixa.Application;

using Microsoft.Extensions.DependencyInjection;

namespace FluxoCaixa.RabbitMQGateway;

/// <summary>
/// Extensões para <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adiciona suporte RabbitMQ para <see cref="IMessageBrokerGateway"/>
    /// </summary>
    public static void AddRabbitMQMessageBrokerGateway(this IServiceCollection services)
    {
        services.AddTransient<IMessageBrokerGateway, RabbitMQMessageBrokerGateway>();
    }
}