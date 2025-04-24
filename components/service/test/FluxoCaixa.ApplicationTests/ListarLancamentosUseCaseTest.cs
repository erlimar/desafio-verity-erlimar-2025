using FluxoCaixa.Application;
using FluxoCaixa.Application.ListarLancamentos;

using Moq;

namespace FluxoCaixa.ApplicationTests;

[Trait("Target", nameof(ListarLancamentosUseCase))]

public class ListarLancamentosUseCaseTest
{
    [Fact(DisplayName = "O caso de uso \"Visualizar lançamentos\" requer um [ILancamentoAppRepository]")]
    public void CasoDeUsoRequerUmRepositorioDeLancamentoAsync()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new ListarLancamentosUseCase(null!));

        Assert.Equal("lancamentoAppRepository", exception.ParamName);
    }

    [Fact(DisplayName = "O caso de uso \"Visualizar lançamentos\" requer dados de entrada")]
    public async Task CasoDeUsoRequerDadosDeEntradaAsync()
    {
        var lancamentoAppRepository = new Mock<ILancamentoAppRepository>().Object;
        var useCase = new ListarLancamentosUseCase(lancamentoAppRepository);

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => useCase.ExecAsync(null!, CancellationToken.None));

        Assert.Equal("form", exception.ParamName);
    }

    [Theory(DisplayName = "O caso de uso \"Visualizar lançamentos\"  requer identificador do dono")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task CasoDeUsoRequerIdentificadorDeDonoValidoAsync(string? identificadorInvalido)
    {
        var lancamentoAppRepository = new Mock<ILancamentoAppRepository>().Object;
        var useCase = new ListarLancamentosUseCase(lancamentoAppRepository);

        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => useCase.ExecAsync(new(
                identificadorInvalido!,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                0), CancellationToken.None));

        Assert.Equal("IdentificadorDono", exception.ParamName);
        Assert.Contains("O identificador do dono não pode ser vazio", exception.Message);
    }

    [Fact(DisplayName = "O caso de uso \"Visualizar lançamentos\" requer uma data final à partir da data inicial")]
    public async Task CasoDeUsoRequerDataFinalMaiorOuIgualDataInicial()
    {
        var lancamentoAppRepository = new Mock<ILancamentoAppRepository>().Object;
        var useCase = new ListarLancamentosUseCase(lancamentoAppRepository);

        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => useCase.ExecAsync(new(
                "identificador-valido",
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddDays(-1),
                0), CancellationToken.None));

        Assert.Equal("PeriodoFinal", exception.ParamName);
        Assert.Contains("Você deve informar um período final maior ou igual ao período inicial", exception.Message);
    }

    [Fact(DisplayName = "O caso de uso \"Visualizar lançamentos\" só permite intervalos de até 90 dias")]
    public async Task CasoDeUsoSoPermiteIntervalosDeAte90Dias()
    {
        var lancamentoAppRepository = new Mock<ILancamentoAppRepository>().Object;
        var useCase = new ListarLancamentosUseCase(lancamentoAppRepository);

        var exception = await Assert.ThrowsAsync<PesquisaPeriodoMaiorQue90DiasException>(
            () => useCase.ExecAsync(new(
                "identificador-valido",
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddDays(91),
                0), CancellationToken.None));

        Assert.Contains("Você só pode pesquisar lançamentos em um período de até 90 dias", exception.Message);
    }

    [Fact(DisplayName = "O caso de uso \"Visualizar lançamentos\" requer um offset à partir de zero")]
    public async Task CasoDeUsoRequerOffsetMaiorOuIgualZero()
    {
        var lancamentoAppRepository = new Mock<ILancamentoAppRepository>().Object;
        var useCase = new ListarLancamentosUseCase(lancamentoAppRepository);

        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => useCase.ExecAsync(new(
                "identificador-valido",
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                -1), CancellationToken.None));

        Assert.Equal("Offset", exception.ParamName);
        Assert.Contains("Informe um Offset à partir de zero", exception.Message);
    }

    [Fact(DisplayName = "O caso de uso \"Visualizar lançamentos\" deve listar do repositório")]
    public async Task CasoDeUsoDeveListarDoRepositorio()
    {
        var lancamentoAppRepositoryMock = new Mock<ILancamentoAppRepository>();
        var useCase = new ListarLancamentosUseCase(lancamentoAppRepositoryMock.Object);

        var periodoInicial = DateTimeOffset.UtcNow.AddDays(-1);
        var periodoFinal = DateTimeOffset.UtcNow.AddDays(1);

        var resultado = await useCase.ExecAsync(new(
            "identificador-valido",
            periodoInicial,
            periodoFinal,
            3), CancellationToken.None);

        lancamentoAppRepositoryMock.Verify(m => m.ListarLancamentosAsync(
            "identificador-valido",
            periodoInicial,
            periodoFinal,
            3), Times.Once);
    }
}
