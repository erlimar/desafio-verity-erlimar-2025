using FluxoCaixa.Application.RegistrarLancamento;

namespace FluxoCaixa.Application;

/// <summary>
/// Corretor para mensagens da aplicação
/// </summary>
public interface IAppMessageBroker
{
    /// <summary>
    /// Envia mensagem <see cref="ConsolidarMessage"/>
    /// </summary>
    /// <param name="message">Dados da mensagem</param>
    Task SendAsync(ConsolidarMessage message, CancellationToken cancellationToken);
}