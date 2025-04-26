using FluxoCaixa.Application;
using FluxoCaixa.Application.CalcularConsolidacao;

using Moq;

namespace FluxoCaixa.ApplicationTests;

[Trait("sut", nameof(CalcularConsolidacaoUseCase))]
public class CalcularConsolidacaoUseCaseTest
{
    [Fact(DisplayName = """O caso de uso "Calcular consolidação diária" requer um [ILancamentoAppRepository]""")]
    public void RequerUmRepositorioDeLancamento()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new CalcularConsolidacaoUseCase(
                null!,
                new Mock<IConsolidadoAppRepository>().Object));

        Assert.Equal("lancamentoAppRepository", exception.ParamName);
    }

    [Fact(DisplayName = """O caso de uso "Calcular consolidação diária" requer um [IConsolidadoAppRepository]""")]
    public void RequerUmRepositorioDeConsolidado()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new CalcularConsolidacaoUseCase(
                new Mock<ILancamentoAppRepository>().Object,
                null!));

        Assert.Equal("consolidadoAppRepository", exception.ParamName);
    }

    [Fact(DisplayName = """O caso de uso "Calcular consolidação diária" requer dados de entrada""")]
    public async Task RequerDadosDeEntradaAsync()
    {
        var useCase = new CalcularConsolidacaoUseCase(
            new Mock<ILancamentoAppRepository>().Object,
            new Mock<IConsolidadoAppRepository>().Object);
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => useCase.ExecAsync(null!, CancellationToken.None));

        Assert.Equal("form", exception.ParamName);
    }

    [Theory(DisplayName = """O caso de uso "Calcular consolidação diária" requer identificador do consolidado""")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task RequerIdentificadorDoConsolidadoValidoAsync(string? identificadorInvalido)
    {
        var useCase = new CalcularConsolidacaoUseCase(
            new Mock<ILancamentoAppRepository>().Object,
            new Mock<IConsolidadoAppRepository>().Object);
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => useCase.ExecAsync(new(identificadorInvalido!), CancellationToken.None));

        Assert.Equal("ConsolidadoId", exception.ParamName);
        Assert.Contains("O identificador do consolidado não pode ser vazio", exception.Message);
    }

    [Fact(DisplayName = """O caso de uso "Calcular consolidação diária" levanta exceção se consolidado não existir""")]
    public async Task RegistroConsolidadoInexistenteLevantaExcecaoAsync()
    {
        var consolidadoAppRepoMock = new Mock<IConsolidadoAppRepository>();

        consolidadoAppRepoMock.Setup(s => s.ObterPorIdAsync(
            "id-inexistente",
            It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<Consolidado?>(null));

        var useCase = new CalcularConsolidacaoUseCase(
            new Mock<ILancamentoAppRepository>().Object,
            consolidadoAppRepoMock.Object);

        var exception = await Assert.ThrowsAsync<ConsolidadoNaoExisteException>(
            () => useCase.ExecAsync(new("id-inexistente"), CancellationToken.None));

        Assert.Equal(
            """O consolidado de identificador "id-inexistente" não existe""",
            exception.Message);

        consolidadoAppRepoMock.Verify(m => m.ObterPorIdAsync(
            "id-inexistente",
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = """O caso de uso "Calcular consolidação diária" deve buscar soma de saldo anterior""")]
    public async Task DeveBuscarSomaDeSaldoAnteriorNoRepositorioAsync()
    {
        var lancamentoAppRepositoryMock = new Mock<ILancamentoAppRepository>();
        var consolidadoAppRepositoryMock = new Mock<IConsolidadoAppRepository>();

        var dataHora = new DateTimeOffset(1983, 8, 8, 23, 59, 59, TimeSpan.Zero);
        var dataDia = new DateTimeOffset(1983, 8, 8, 0, 0, 0, TimeSpan.Zero);

        consolidadoAppRepositoryMock.Setup(s => s.ObterPorIdAsync(
            "id-existente",
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Consolidado()
            {
                Id = "id-existente",
                DataHora = dataHora,
                Status = StatusConsolidado.Pendente,
                IdentificadorDono = "identificador-dono",
                SaldoAnterior = 0m,
                SaldoFinal = 0m,
                SaldoPeriodo = 0m
            });

        var useCase = new CalcularConsolidacaoUseCase(
            lancamentoAppRepositoryMock.Object,
            consolidadoAppRepositoryMock.Object);

        _ = await useCase.ExecAsync(new("id-existente"), CancellationToken.None);

        consolidadoAppRepositoryMock.Verify(m => m.ObterPorIdAsync(
            "id-existente",
            It.IsAny<CancellationToken>()), Times.Once);

        lancamentoAppRepositoryMock.Verify(m => m.SomarValoresLancamentosAntesDeAsync(
            "identificador-dono",
            dataDia,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = """O caso de uso "Calcular consolidação diária" deve buscar soma de saldo do dia""")]
    public async Task DeveBuscarSomaDeSaldoDiaNoRepositorioAsync()
    {
        var lancamentoAppRepositoryMock = new Mock<ILancamentoAppRepository>();
        var consolidadoAppRepositoryMock = new Mock<IConsolidadoAppRepository>();

        var dataHora = new DateTimeOffset(1983, 8, 8, 23, 59, 59, TimeSpan.Zero);
        var dataDia = new DateTimeOffset(1983, 8, 8, 0, 0, 0, TimeSpan.Zero);

        consolidadoAppRepositoryMock.Setup(s => s.ObterPorIdAsync(
            "id-existente",
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Consolidado()
            {
                Id = "id-existente",
                DataHora = dataHora,
                Status = StatusConsolidado.Pendente,
                IdentificadorDono = "identificador-dono",
                SaldoAnterior = 0m,
                SaldoFinal = 0m,
                SaldoPeriodo = 0m
            });

        var useCase = new CalcularConsolidacaoUseCase(
            lancamentoAppRepositoryMock.Object,
            consolidadoAppRepositoryMock.Object);

        _ = await useCase.ExecAsync(new("id-existente"), CancellationToken.None);

        consolidadoAppRepositoryMock.Verify(m => m.ObterPorIdAsync(
            "id-existente",
            It.IsAny<CancellationToken>()), Times.Once);

        lancamentoAppRepositoryMock.Verify(m => m.SomarValoresLancamentosDoDiaAsync(
            "identificador-dono",
            dataDia,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = """O caso de uso "Calcular consolidação diária" deve calcular saldo final corretamente""")]
    [InlineData(100.30, 23.0, 123.30)]
    [InlineData(-50, 50, 0)]
    [InlineData(444.44444444, 555.55555555, 999.99999999)]
    [InlineData(-444.44444444, 555.55555555, 111.11111111)]
    [InlineData(444.44444444, -555.55555555, -111.11111111)]
    public async Task DeveCalcularSaldoFinalAsync(decimal saldoAnterior, decimal saldoPeriodo, decimal saldoFinal)
    {
        var lancamentoAppRepositoryMock = new Mock<ILancamentoAppRepository>();
        var consolidadoAppRepositoryMock = new Mock<IConsolidadoAppRepository>();

        lancamentoAppRepositoryMock.Setup(s => s.SomarValoresLancamentosAntesDeAsync(
            It.IsAny<string>(),
            It.IsAny<DateTimeOffset>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(saldoAnterior);

        lancamentoAppRepositoryMock.Setup(s => s.SomarValoresLancamentosDoDiaAsync(
            It.IsAny<string>(),
            It.IsAny<DateTimeOffset>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(saldoPeriodo);

        consolidadoAppRepositoryMock.Setup(s => s.ObterPorIdAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Mock<Consolidado>().Object);

        var useCase = new CalcularConsolidacaoUseCase(
            lancamentoAppRepositoryMock.Object,
            consolidadoAppRepositoryMock.Object);

        var resultado = await useCase.ExecAsync(new("id-existente"), CancellationToken.None);

        Assert.Equal(saldoFinal, resultado.SaldoFinal);
    }

    [Fact(DisplayName = """O caso de uso "Calcular consolidação diária" deve gravar o resultado que for retornado""")]
    public async Task DeveGravarResultadoAsync()
    {
        var lancamentoAppRepositoryMock = new Mock<ILancamentoAppRepository>();
        var consolidadoAppRepositoryMock = new Mock<IConsolidadoAppRepository>();

        consolidadoAppRepositoryMock.Setup(s => s.ObterPorIdAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Mock<Consolidado>().Object);

        var useCase = new CalcularConsolidacaoUseCase(
            lancamentoAppRepositoryMock.Object,
            consolidadoAppRepositoryMock.Object);

        var resultado = await useCase.ExecAsync(new("id-existente"), CancellationToken.None);

        consolidadoAppRepositoryMock.Verify(m => m.GravarConsolidadoAsync(
            resultado,
            It.IsAny<CancellationToken>()), Times.Once);
    }
}