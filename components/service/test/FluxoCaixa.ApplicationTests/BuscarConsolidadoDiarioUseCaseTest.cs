using FluxoCaixa.Application;
using FluxoCaixa.Application.BuscarConsolidadoDiario;
using FluxoCaixa.Application.ListarLancamentos;

using Moq;

namespace FluxoCaixa.ApplicationTests;

[Trait("sut", nameof(BuscarConsolidadoDiarioUseCase))]
public class BuscarConsolidadoDiarioUseCaseTest
{
    [Fact(DisplayName = """O caso de uso "Buscar consolidado diário" requer um [IConsolidadoAppRepository]""")]
    public void RequerUmRepositorioDeConsolidado()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new BuscarConsolidadoDiarioUseCase(null!));

        Assert.Equal("consolidadoAppRepository", exception.ParamName);
    }

    [Fact(DisplayName = """O caso de uso "Buscar consolidado diário" requer dados de entrada""")]
    public async Task RequerDadosDeEntradaAsync()
    {
        var consolidadoAppRepository = new Mock<IConsolidadoAppRepository>().Object;
        var useCase = new BuscarConsolidadoDiarioUseCase(consolidadoAppRepository);

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => useCase.ExecAsync(null!, CancellationToken.None));

        Assert.Equal("form", exception.ParamName);
    }

    [Theory(DisplayName = """O caso de uso "Buscar consolidado diário"  requer identificador do dono""")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task RequerIdentificadorDeDonoValidoAsync(string? identificadorInvalido)
    {
        var consolidadoAppRepository = new Mock<IConsolidadoAppRepository>().Object;
        var useCase = new BuscarConsolidadoDiarioUseCase(consolidadoAppRepository);

        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => useCase.ExecAsync(new(
                identificadorInvalido!,
                DateTimeOffset.UtcNow), CancellationToken.None));

        Assert.Equal("IdentificadorDono", exception.ParamName);
        Assert.Contains("O identificador do dono não pode ser vazio", exception.Message);
    }

    [Fact(DisplayName = """O caso de uso "Buscar consolidado diário" não considera a hora na busca""")]
    public async Task NaoConsideraHoraNaBuscasync()
    {
        var consolidadoAppRepositoryMock = new Mock<IConsolidadoAppRepository>();

        var useCase = new BuscarConsolidadoDiarioUseCase(consolidadoAppRepositoryMock.Object);

        var dataHora = new DateTimeOffset(1983, 8, 8, 1, 2, 3, TimeSpan.Zero);
        var dataSemHora = new DateTimeOffset(1983, 8, 8, 0, 0, 0, TimeSpan.Zero);

        _ = await useCase.ExecAsync(new(
            "identificador-valido",
            dataHora), CancellationToken.None);

        consolidadoAppRepositoryMock.Verify(m => m.ObterConsolidadosDaDataAsync(
            "identificador-valido",
            dataSemHora,
            CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = """O caso de uso "Buscar consolidado diário" retorna apenas o primeiro registro""")]
    public async Task RetornaApenasPrimeiroRegistroAsync()
    {
        var primeiroRegistro = new Mock<Consolidado>().Object;
        var segundoRegistro = new Mock<Consolidado>().Object;

        var consolidadoAppRepositoryMock = new Mock<IConsolidadoAppRepository>();

        consolidadoAppRepositoryMock.Setup(s => s.ObterConsolidadosDaDataAsync(
            It.IsAny<string>(),
            It.IsAny<DateTimeOffset>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync([primeiroRegistro, segundoRegistro]);

        var useCase = new BuscarConsolidadoDiarioUseCase(consolidadoAppRepositoryMock.Object);

        var resultado = await useCase.ExecAsync(new(
            "identificador-valido",
            DateTimeOffset.UtcNow), CancellationToken.None);

        Assert.Same(primeiroRegistro, resultado);
        Assert.NotSame(primeiroRegistro, segundoRegistro);
        Assert.NotSame(segundoRegistro, resultado);
    }
}