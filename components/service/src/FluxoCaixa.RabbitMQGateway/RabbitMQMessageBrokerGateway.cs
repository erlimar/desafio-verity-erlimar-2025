using FluxoCaixa.Application;
using FluxoCaixa.Application.RegistrarLancamento;

namespace FluxoCaixa.RabbitMQGateway;

/// <summary>
/// Gateway de mensagens para RabbitMQ
/// </summary>
public class RabbitMQMessageBrokerGateway : IMessageBrokerGateway
{
    public Task SendAsync(ConsolidarMessage message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}