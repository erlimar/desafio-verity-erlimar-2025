using FluxoCaixa.Application.RegistrarLancamento;

namespace FluxoCaixa.Application;

/// <summary>
/// Adaptador de comunicação com "message broker"
/// </summary>
public interface IMessageBrokerGateway
{
    /// <summary>
    /// Envia mensagem <see cref="ConsolidarMessage"/>
    /// </summary>
    /// <param name="message">Dados da mensagem</param>
    Task SendAsync(ConsolidarMessage message, CancellationToken cancellationToken);
}