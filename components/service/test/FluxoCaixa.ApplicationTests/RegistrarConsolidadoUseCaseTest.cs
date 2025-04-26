using FluxoCaixa.Application;
using FluxoCaixa.Application.Models;
using FluxoCaixa.Application.RegistrarConsolidado;

using Moq;

namespace FluxoCaixa.ApplicationTests;

[Trait("sut", nameof(RegistrarConsolidadoUseCase))]
public class RegistrarConsolidadoUseCaseTest
{
    [Fact(DisplayName = """O caso de uso "Registrar consolidado" requer um [IConsolidadoAppRepository]""")]
    public void RequerUmRepositorioDeConsolidado()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new RegistrarConsolidadoUseCase(null!));

        Assert.Equal("consolidadoAppRepository", exception.ParamName);
    }

    [Fact(DisplayName = """O caso de uso "Registrar consolidado" requer dados de entrada""")]
    public async Task RequerDadosDeEntradaAsync()
    {
        var useCase = new RegistrarConsolidadoUseCase(new Mock<IConsolidadoAppRepository>().Object);
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => useCase.ExecAsync(null!, CancellationToken.None));

        Assert.Equal("form", exception.ParamName);
    }

    [Theory(DisplayName = """O caso de uso "Registrar consolidado" requer identificador do dono""")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task RequerIdentificadorDeDonoValidoAsync(string? identificadorInvalido)
    {
        var consolidadoAppRepository = new Mock<IConsolidadoAppRepository>().Object;
        var useCase = new RegistrarConsolidadoUseCase(consolidadoAppRepository);
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => useCase.ExecAsync(new(
                identificadorInvalido!,
                DateTimeOffset.UtcNow), CancellationToken.None));

        Assert.Equal("IdentificadorDono", exception.ParamName);
        Assert.Contains("O identificador do dono não pode ser vazio", exception.Message);
    }

    [Fact(DisplayName = """O caso de uso "Registrar consolidado" usa apenas data no registro""")]
    public async Task NaConsultaSoDataSemHoraUsada()
    {
        var consolidadoAppRepositoryMock = new Mock<IConsolidadoAppRepository>();

        var useCase = new RegistrarConsolidadoUseCase(consolidadoAppRepositoryMock.Object);

        var dataHora = new DateTimeOffset(1983, 8, 8, 1, 2, 3, TimeSpan.Zero);
        var dataApenas = new DateTimeOffset(1983, 8, 8, 0, 0, 0, TimeSpan.Zero);

        await useCase.ExecAsync(new("id", dataHora), CancellationToken.None);

        consolidadoAppRepositoryMock.Verify(m => m.ObterConsolidadosDaDataAsync(
            "id",
            dataApenas,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = """O caso de uso "Registrar consolidado" só permite um por data""")]
    public async Task SoUmConsolidadoPorData()
    {
        var consolidadoAppRepositoryMock = new Mock<IConsolidadoAppRepository>();

        consolidadoAppRepositoryMock.Setup(s => s.ObterConsolidadosDaDataAsync(
            It.IsAny<string>(),
            It.IsAny<DateTimeOffset>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync([new Mock<ConsolidadoModel>().Object]);

        var useCase = new RegistrarConsolidadoUseCase(consolidadoAppRepositoryMock.Object);
        var exception = await Assert.ThrowsAsync<ConsolidadoJaExisteException>(
            () => useCase.ExecAsync(new(
                "id",
                DateTimeOffset.UtcNow), CancellationToken.None));

        Assert.Contains("á existe um consolidado para a data informada", exception.Message);
    }

    [Fact(DisplayName = """O caso de uso "Registrar consolidado" deve registrar no repositório""")]
    public async Task DeveRegistrarNoRepositorio()
    {
        var consolidadoAppRepositoryMock = new Mock<IConsolidadoAppRepository>();

        consolidadoAppRepositoryMock.Setup(s => s.ObterConsolidadosDaDataAsync(
            It.IsAny<string>(),
            It.IsAny<DateTimeOffset>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var useCase = new RegistrarConsolidadoUseCase(consolidadoAppRepositoryMock.Object);

        var dataHora = new DateTimeOffset(1983, 8, 8, 1, 2, 3, TimeSpan.Zero);
        var dataApenas = new DateTimeOffset(1983, 8, 8, 0, 0, 0, TimeSpan.Zero);

        var model = new ConsolidadoModel()
        {
            Id = string.Empty,
            IdentificadorDono = "id",
            DataHora = dataApenas,
            Status = StatusConsolidado.Pendente,
            SaldoAnterior = 0,
            SaldoPeriodo = 0,
            SaldoFinal = 0
        };

        await useCase.ExecAsync(new("id", dataHora), CancellationToken.None);

        consolidadoAppRepositoryMock.Verify(m => m.GravarConsolidadoAsync(
            model,
            CancellationToken.None), Times.Once);
    }
}