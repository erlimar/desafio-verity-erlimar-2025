using FluxoCaixa.Application;
using FluxoCaixa.Application.RegistrarLancamento;

using Moq;

namespace FluxoCaixa.ApplicationTests;

[Trait("sut", nameof(RegistrarLancamentoUseCase))]
public class RegistrarLancamentoUseCaseTest
{
    [Fact(DisplayName = """O caso de uso "Registrar Lançamento" requer um [IIdentityProviderGateway]""")]
    public void RequerUmGatewayDoIdentityProvider()
    {
        var lancamentoAppRepository = new Mock<ILancamentoAppRepository>().Object;
        var consolidadoAppRepository = new Mock<IConsolidadoAppRepository>().Object;
        var appMessageBroker = new Mock<IAppMessageBroker>().Object;
        var exception = Assert.Throws<ArgumentNullException>(
            () => new RegistrarLancamentoUseCase(null!,
                lancamentoAppRepository,
                consolidadoAppRepository,
                appMessageBroker));

        Assert.Equal("identityProviderGateway", exception.ParamName);
    }

    [Fact(DisplayName = """O caso de uso "Registrar Lançamento" requer um [ILancamentoAppRepository]""")]
    public void RequerUmRepositorioDeLancamento()
    {
        var identityProviderGateway = new Mock<IIdentityProviderGateway>().Object;
        var consolidadoAppRepository = new Mock<IConsolidadoAppRepository>().Object;
        var appMessageBroker = new Mock<IAppMessageBroker>().Object;
        var exception = Assert.Throws<ArgumentNullException>(
            () => new RegistrarLancamentoUseCase(
                identityProviderGateway,
                null!,
                consolidadoAppRepository,
                appMessageBroker));

        Assert.Equal("lancamentoAppRepository", exception.ParamName);
    }

    [Fact(DisplayName = """O caso de uso "Registrar Lançamento" requer um [IAppMessageBroker]""")]
    public void RequerUmCorretorDeMensagens()
    {
        var identityProviderGateway = new Mock<IIdentityProviderGateway>().Object;
        var consolidadoAppRepository = new Mock<IConsolidadoAppRepository>().Object;
        var lancamentoAppRepository = new Mock<ILancamentoAppRepository>().Object;
        var exception = Assert.Throws<ArgumentNullException>(
            () => new RegistrarLancamentoUseCase(
                identityProviderGateway,
                lancamentoAppRepository,
                consolidadoAppRepository,
                null!));

        Assert.Equal("appMessageBroker", exception.ParamName);
    }

    [Fact(DisplayName = """O caso de uso "Registrar Lançamento" requer um [IConsolidadoAppRepository]""")]
    public void RequerUmRepositorioDeConsolidado()
    {
        var identityProviderGateway = new Mock<IIdentityProviderGateway>().Object;
        var lancamentoAppRepository = new Mock<ILancamentoAppRepository>().Object;
        var appMessageBroker = new Mock<IAppMessageBroker>().Object;
        var exception = Assert.Throws<ArgumentNullException>(
            () => new RegistrarLancamentoUseCase(
                identityProviderGateway,
                lancamentoAppRepository,
                null!,
                appMessageBroker));

        Assert.Equal("consolidadoAppRepository", exception.ParamName);
    }

    [Fact(DisplayName = """O caso de uso "Registrar Lançamento" requer dados de entrada""")]
    public async Task RequerDadosDeEntradaAsync()
    {
        var identityProviderGateway = new Mock<IIdentityProviderGateway>().Object;
        var lancamentoAppRepository = new Mock<ILancamentoAppRepository>().Object;
        var consolidadoAppRepository = new Mock<IConsolidadoAppRepository>().Object;
        var appMessageBroker = new Mock<IAppMessageBroker>().Object;
        var useCase = new RegistrarLancamentoUseCase(
            identityProviderGateway,
            lancamentoAppRepository,
            consolidadoAppRepository,
            appMessageBroker);
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => useCase.ExecAsync(null!, CancellationToken.None));

        Assert.Equal("form", exception.ParamName);
    }

    [Theory(DisplayName = """O caso de uso "Registrar Lançamento" requer identificador do dono""")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task RequerIdentificadorDeDonoValidoAsync(string? identificadorInvalido)
    {
        var identityProviderGateway = new Mock<IIdentityProviderGateway>().Object;
        var lancamentoAppRepository = new Mock<ILancamentoAppRepository>().Object;
        var consolidadoAppRepository = new Mock<IConsolidadoAppRepository>().Object;
        var appMessageBroker = new Mock<IAppMessageBroker>().Object;
        var useCase = new RegistrarLancamentoUseCase(
            identityProviderGateway,
            lancamentoAppRepository,
            consolidadoAppRepository,
            appMessageBroker);
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => useCase.ExecAsync(new(identificadorInvalido!,
                TipoLancamento.Credito,
                DateTimeOffset.UtcNow,
                decimal.MinValue,
                "Descrição"), CancellationToken.None));

        Assert.Equal("IdentificadorDono", exception.ParamName);
        Assert.Contains("O identificador do dono não pode ser vazio", exception.Message);
    }

    [Theory(DisplayName = """O caso de uso "Registrar Lançamento" requer uma descrição""")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task RequerDescricaoValidaAsync(string? descricaoInvalida)
    {
        var identityProviderGateway = new Mock<IIdentityProviderGateway>().Object;
        var lancamentoAppRepository = new Mock<ILancamentoAppRepository>().Object;
        var consolidadoAppRepository = new Mock<IConsolidadoAppRepository>().Object;
        var appMessageBroker = new Mock<IAppMessageBroker>().Object;
        var useCase = new RegistrarLancamentoUseCase(
            identityProviderGateway,
            lancamentoAppRepository,
            consolidadoAppRepository,
            appMessageBroker);
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => useCase.ExecAsync(new("Dono",
                TipoLancamento.Credito,
                DateTimeOffset.UtcNow,
                decimal.MinValue,
                descricaoInvalida!), CancellationToken.None));

        Assert.Equal("Descricao", exception.ParamName);
        Assert.Contains("A descrição não pode ser vazia", exception.Message);
    }

    [Theory(DisplayName = """O caso de uso "Registrar Lançamento" requer valor maior que zero""")]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-79235)]
    [InlineData(-0.000000000001)]
    public async Task RequerValorMaiorQueZeroAsync(decimal valorInvalido)
    {
        var identityProviderGateway = new Mock<IIdentityProviderGateway>().Object;
        var lancamentoAppRepository = new Mock<ILancamentoAppRepository>().Object;
        var consolidadoAppRepository = new Mock<IConsolidadoAppRepository>().Object;
        var appMessageBroker = new Mock<IAppMessageBroker>().Object;
        var useCase = new RegistrarLancamentoUseCase(
            identityProviderGateway,
            lancamentoAppRepository,
            consolidadoAppRepository,
            appMessageBroker);
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => useCase.ExecAsync(new("Dono",
                TipoLancamento.Credito,
                DateTimeOffset.UtcNow,
                valorInvalido,
                "Descricao"), CancellationToken.None));

        Assert.Equal("Valor", exception.ParamName);
        Assert.Contains("O valor precisa ser maior que zero", exception.Message);
    }

    [Fact(DisplayName = """O caso de uso "Registrar Lançamento" requer que o dono exista""")]
    public async Task ODonoDeveExistirAsync()
    {
        var identityProviderMock = new Mock<IIdentityProviderGateway>();

        identityProviderMock.Setup(s => s.UsuarioExisteAsync(
            "codigo-nao-existente",
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var lancamentoAppRepository = new Mock<ILancamentoAppRepository>().Object;
        var consolidadoAppRepository = new Mock<IConsolidadoAppRepository>().Object;
        var appMessageBroker = new Mock<IAppMessageBroker>().Object;

        var useCase = new RegistrarLancamentoUseCase(
            identityProviderMock.Object,
            lancamentoAppRepository,
            consolidadoAppRepository,
            appMessageBroker);
        var exception = await Assert.ThrowsAsync<DonoLancamentoInvalidoException>(
            () => useCase.ExecAsync(new("codigo-nao-existente",
                TipoLancamento.Credito,
                DateTimeOffset.UtcNow,
                1,
                "Descricao"), CancellationToken.None));

        Assert.Equal("""O dono de lançamento "codigo-nao-existente" informado não é válido""", exception.Message);

        identityProviderMock.Verify(m => m.UsuarioExisteAsync(
            "codigo-nao-existente",
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = """O caso de uso "Registrar Lançamento" não permite lançamento repetido""")]
    public async Task NaoDevePermitirLancamentoRepetidoAsync()
    {
        var identityProviderMock = new Mock<IIdentityProviderGateway>();

        identityProviderMock.Setup(s => s.UsuarioExisteAsync("codigo-dono", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var lancamentoAppRepositoryMock = new Mock<ILancamentoAppRepository>();

        lancamentoAppRepositoryMock.Setup(s => s.ContarLancamentosPorFiltroAsync(
            It.IsAny<string>(),
            It.IsAny<LancamentoFilter>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var consolidadoAppRepositoryMock = new Mock<IConsolidadoAppRepository>();
        var appMessageBrokerMock = new Mock<IAppMessageBroker>();

        var useCase = new RegistrarLancamentoUseCase(
            identityProviderMock.Object,
            lancamentoAppRepositoryMock.Object,
            consolidadoAppRepositoryMock.Object,
            appMessageBrokerMock.Object);
        var exception = await Assert.ThrowsAsync<LancamentoRepetidoException>(
            () => useCase.ExecAsync(new("codigo-dono",
                TipoLancamento.Credito,
                DateTimeOffset.UtcNow,
                1,
                "Descrição repetida"), CancellationToken.None));

        Assert.Equal("Este lançamento já parece ter sido feito antes", exception.Message);

        identityProviderMock.Verify(m => m.UsuarioExisteAsync(
            "codigo-dono",
            It.IsAny<CancellationToken>()), Times.Once);
        lancamentoAppRepositoryMock.Verify(m => m.ContarLancamentosPorFiltroAsync(
            It.IsAny<string>(),
            It.IsAny<LancamentoFilter>(),
            It.IsAny<CancellationToken>()), Times.Once);
        consolidadoAppRepositoryMock.Verify(m => m.ObterConsolidadosAPartirDoDiaAsync(
            "codigo-dono",
            It.IsAny<DateTimeOffset>(),
            It.IsAny<CancellationToken>()), Times.Never);
        appMessageBrokerMock.Verify(m => m.SendAsync(
            It.IsAny<ConsolidarMessage>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = """O caso de uso "Registrar Lançamento" invalida todos os consolidados à partir do dia do lançamento""")]
    public async Task DeveInvalidarConsolidadosCasoExistamAsync()
    {
        var identityProviderMock = new Mock<IIdentityProviderGateway>();

        identityProviderMock.Setup(s => s.UsuarioExisteAsync(
            "codigo-dono",
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var lancamentoAppRepositoryMock = new Mock<ILancamentoAppRepository>();

        lancamentoAppRepositoryMock.Setup(s => s.ContarLancamentosPorFiltroAsync(
            It.IsAny<string>(),
            It.IsAny<LancamentoFilter>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        var consolidadoAppRepositoryMock = new Mock<IConsolidadoAppRepository>();

        consolidadoAppRepositoryMock.Setup(s => s.ObterConsolidadosAPartirDoDiaAsync(
            "codigo-dono",
            It.IsAny<DateTimeOffset>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                new()
                {
                    IdentificadorDono = string.Empty,
                    Status = StatusConsolidado.Pendente,
                    DataHora = DateTimeOffset.UtcNow
                },
                new()
                {
                    IdentificadorDono = string.Empty,
                    Status = StatusConsolidado.Pendente,
                    DataHora = DateTimeOffset.UtcNow
                }
            ]);

        var appMessageBrokerMock = new Mock<IAppMessageBroker>();

        var useCase = new RegistrarLancamentoUseCase(
            identityProviderMock.Object,
            lancamentoAppRepositoryMock.Object,
            consolidadoAppRepositoryMock.Object,
            appMessageBrokerMock.Object);

        await useCase.ExecAsync(new(
            "codigo-dono",
            TipoLancamento.Credito,
            DateTimeOffset.UtcNow,
            1,
            "Descrição repetida"), It.IsAny<CancellationToken>());

        identityProviderMock.Verify(m => m.UsuarioExisteAsync(
            "codigo-dono",
            It.IsAny<CancellationToken>()), Times.Once);
        lancamentoAppRepositoryMock.Verify(m => m.ContarLancamentosPorFiltroAsync(
            It.IsAny<string>(),
            It.IsAny<LancamentoFilter>(),
            It.IsAny<CancellationToken>()), Times.Once);
        lancamentoAppRepositoryMock.Verify(m => m.GravarLancamentoAsync(
            It.IsAny<Lancamento>(),
            It.IsAny<CancellationToken>()), Times.Once);
        consolidadoAppRepositoryMock.Verify(m => m.ObterConsolidadosAPartirDoDiaAsync(
            "codigo-dono",
            It.IsAny<DateTimeOffset>(),
            It.IsAny<CancellationToken>()), Times.Once);
        consolidadoAppRepositoryMock.Verify(m => m.GravarConsolidadoAsync(
            It.IsAny<Consolidado>(),
            It.IsAny<CancellationToken>()), Times.Exactly(2));
        appMessageBrokerMock.Verify(m => m.SendAsync(
            It.IsAny<ConsolidarMessage>(),
            It.IsAny<CancellationToken>()), Times.Exactly(2));
    }
}