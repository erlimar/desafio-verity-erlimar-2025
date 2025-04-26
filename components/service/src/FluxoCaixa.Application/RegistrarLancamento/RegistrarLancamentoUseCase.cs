namespace FluxoCaixa.Application.RegistrarLancamento;

/// <summary>
/// Executa o caso de uso "Registrar Lançamento", que cadastra um novo lançamento
/// </summary>
public class RegistrarLancamentoUseCase
    : IUseCaseInputOnly<RegistrarLancamentoForm>
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
    /// <exception cref="ArgumentNullException">
    /// Quando os dados de entrada são inválidos
    /// </exception>
    /// <exception cref="DonoLancamentoInvalidoException">
    /// Quando o dono do lançamento não existe
    /// </exception>
    /// <exception cref="LancamentoRepetidoException">
    /// Quando os dados do lançamento estão repedidos em tipo, descrição e
    /// data/hora.
    /// </exception>
    public async Task ExecAsync(RegistrarLancamentoForm form, CancellationToken cancellationToken)
    {
        ValidateForm(form);

        // Regra: O registro deve estar sempre vinculado ao usuário dono
        // TODO: Refatorar para mais explicitação em uma especificação de regra
        if (!await _identityProviderGateway.UsuarioExisteAsync(
            form.IdentificadorDono,
            cancellationToken))
        {
            throw new DonoLancamentoInvalidoException(form.IdentificadorDono);
        }

        // Regra: Não deve ser permitido o registro de mais de um lançamento
        //        com mesmo tipo, descrição, data e hora
        var filtroLancamentosRepetidos = new LancamentoFilter
        {
            DataHora = form.DataHora,
            Tipo = form.Tipo,
            Descricao = form.Descricao
        };

        if (await _lancamentoAppRepository.ContarLancamentosPorFiltroAsync(
            form.IdentificadorDono,
            filtroLancamentosRepetidos,
            cancellationToken) > 0)
        {
            throw new LancamentoRepetidoException();
        }

        // Regra: Caso hajam cosolidações já registradas à partir do dia do
        //        lançamento, essas devem ser invalidadas, e mensagens
        //        correspondentes para o serviço "Consolidado" devem ser emitidas
        //        para que o cálculo seja refeito.
        var consolidacoes = await _consolidadoAppRepository.ObterConsolidadosAPartirDoDiaAsync(
            form.IdentificadorDono,
            form.DataHora,
            cancellationToken) ?? [];

        if (consolidacoes.Any())
        {
            foreach (var consolidacao in consolidacoes)
            {
                await _consolidadoAppRepository.GravarConsolidadoAsync(new Consolidado()
                {
                    Id = consolidacao.Id,
                    IdentificadorDono = consolidacao.IdentificadorDono,
                    DataHora = consolidacao.DataHora,
                    Status = StatusConsolidado.Invalidado
                }, cancellationToken);

                await _appMessageBroker.SendAsync(new ConsolidarMessage()
                {
                    Id = consolidacao.Id!
                }, cancellationToken);
            }
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
            throw new ArgumentException(
                "O identificador do dono não pode ser vazio",
                nameof(form.IdentificadorDono));
        }

        if (string.IsNullOrWhiteSpace(form.Descricao))
        {
            throw new ArgumentException(
                "A descrição não pode ser vazia",
                nameof(form.Descricao));
        }

        if (form.Valor <= 0m)
        {
            throw new ArgumentException(
                "O valor precisa ser maior que zero",
                nameof(form.Valor));
        }
    }
}