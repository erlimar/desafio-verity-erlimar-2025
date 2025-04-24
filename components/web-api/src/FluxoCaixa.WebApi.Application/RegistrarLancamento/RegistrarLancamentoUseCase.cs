namespace FluxoCaixa.WebApi.Application.RegistrarLancamento;

/// <summary>
/// Executa o caso de uso "Registrar Lançamento"
/// </summary>
public class RegistrarLancamentoUseCase
    : IUseCaseWithInputForm<RegistrarLancamentoForm>
{
    private readonly IIdentityProviderGateway _identityProviderGateway;
    private readonly ILancamentoAppRepository _lancamentoAppRepository;
    private readonly IConsolidadoAppRepository _consolidadoAppRepository;
    private readonly IAppMessageBroker _appMessageBroker;

    public RegistrarLancamentoUseCase(
        IIdentityProviderGateway identityProviderGateway,
        ILancamentoAppRepository lancamentoAppRepository,
        IConsolidadoAppRepository consolidadoAppRepository,
        IAppMessageBroker appMessageBroker)
    {
        _identityProviderGateway = identityProviderGateway
            ?? throw new ArgumentNullException(nameof(identityProviderGateway));

        _lancamentoAppRepository = lancamentoAppRepository
            ?? throw new ArgumentNullException(nameof(lancamentoAppRepository));

        _consolidadoAppRepository = consolidadoAppRepository
            ?? throw new ArgumentNullException(nameof(consolidadoAppRepository));

        _appMessageBroker = appMessageBroker
            ?? throw new ArgumentNullException(nameof(appMessageBroker));
    }

    /// <summary>
    /// Executa o caso de uso
    /// </summary>
    /// <param name="form">Dados de entrada</param>
    /// <returns></returns>
    /// <exception cref="DonoLancamentoInvalidoException">
    /// Quando o dono do lançamento não existe
    /// </exception>
    public async Task ExecAsync(RegistrarLancamentoForm form, CancellationToken cancellationToken)
    {
        ValidateForm(form);

        // Regra: O registro deve estar sempre vinculado ao usuário dono
        // TODO: Refatorar para mais explicitação em uma especificação de regra
        if (!await _identityProviderGateway.UsuarioExisteAsync(form.IdentificadorDono, cancellationToken))
        {
            throw new DonoLancamentoInvalidoException(form.IdentificadorDono);
        }

        // Regra: Não deve ser permitido o registro de mais de um lançamento
        //        com mesmo tipo, descrição, data e hora
        var filtroLancamentosRepetidos = new LancamentoFilter
        {
            IdentificadorDono = form.IdentificadorDono,
            DataHora = form.DataHora,
            Tipo = form.Tipo,
            Descricao = form.Descricao
        };

        if (await _lancamentoAppRepository.ObterTotalLancamentosPorFiltroAsync(filtroLancamentosRepetidos, cancellationToken) > 0)
        {
            throw new LancamentoRepetidoException();
        }

        // Regra: Caso haja uma cosolidação já registrada para o dia do lançamento,
        //        essa deve ser invalidada, e uma mensagem para o serviço
        //        Consolidado deve ser emitida para que o cálculo seja refeito
        if (await _consolidadoAppRepository.ExisteConsolidadoDoDiaAsync(form.DataHora, cancellationToken))
        {
            await _consolidadoAppRepository.GravarConsolidadoAsync(new Consolidado()
            {
                IdentificadorDono = form.IdentificadorDono,
                DataHora = form.DataHora,
                Status = StatusConsolidado.Invalidado
            }, cancellationToken);

            await _appMessageBroker.SendAsync(new ConsolidarMessage()
            {
                Data = form.DataHora
            }, cancellationToken);
        }

        await _lancamentoAppRepository.GravarLancamentoAsync(new Lancamento()
        {
            IdentificadorDono = form.IdentificadorDono,
            Tipo = form.Tipo,
            DataHora = form.DataHora,
            Valor = form.Valor,
            Descricao = form.Descricao
        }, cancellationToken);
    }

    private static void ValidateForm(RegistrarLancamentoForm form)
    {
        _ = form ?? throw new ArgumentNullException(nameof(form));

        if (string.IsNullOrWhiteSpace(form.IdentificadorDono))
        {
            throw new ArgumentException("O identificador do dono não pode ser vazio", nameof(form.IdentificadorDono));
        }

        if (string.IsNullOrWhiteSpace(form.Descricao))
        {
            throw new ArgumentException("A descrição não pode ser vazia", nameof(form.Descricao));
        }

        if (form.Valor <= 0m)
        {
            throw new ArgumentException("O valor precisa ser maior que zero", nameof(form.Valor));
        }
    }
}