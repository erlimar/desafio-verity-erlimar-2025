using FluxoCaixa.WebApi.Application.RegistrarLancamento;

namespace FluxoCaixa.WebApi.ApplicationTests;

[Trait("Target", nameof(RegistrarLancamentoUseCase))]
public class RegistrarLancamentoUseCaseTest
{
    [Fact(DisplayName = "O caso de uso \"Registrar Lançamento\" requer dados de entrada")]
    public async Task CasoDeUsoRequerDadosDeEntradaAsync()
    {
        var useCase = new RegistrarLancamentoUseCase();
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => useCase.ExecAsync(null!));

        Assert.Equal("form", exception.ParamName);
    }

    [Theory(DisplayName = "O caso de uso \"Registrar Lançamento\" requer identificador do dono")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task CasoDeUsoRequerIdentificadorDeDonoValidoAsync(string? identificadorInvalido)
    {
        var useCase = new RegistrarLancamentoUseCase();
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => useCase.ExecAsync(new(identificadorInvalido!,
                TipoLancamento.Credito,
                DateTimeOffset.UtcNow,
                decimal.MinValue,
                "Descrição")));

        Assert.Equal("IdentificadorDono", exception.ParamName);
        Assert.Contains("O identificador do dono não pode ser vazio", exception.Message);
    }

    [Theory(DisplayName = "O caso de uso \"Registrar Lançamento\" requer uma descrição")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CasoDeUsoRequerDescricaoValidaAsync(string? descricaoInvalida)
    {
        var useCase = new RegistrarLancamentoUseCase();
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => useCase.ExecAsync(new("Dono",
                TipoLancamento.Credito,
                DateTimeOffset.UtcNow,
                decimal.MinValue,
                descricaoInvalida!)));

        Assert.Equal("Descricao", exception.ParamName);
        Assert.Contains("A descrição não pode ser vazia", exception.Message);
    }

    [Theory(DisplayName = "O caso de uso \"Registrar Lançamento\" requer valor maior que zero")]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-79235)]
    [InlineData(-0.000000000001)]
    public async Task CasoDeUsoRequerValorMaiorQueZeroAsync(decimal valorInvalido)
    {
        var useCase = new RegistrarLancamentoUseCase();
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => useCase.ExecAsync(new("Dono",
                TipoLancamento.Credito,
                DateTimeOffset.UtcNow,
                valorInvalido,
                "Descricao")));

        Assert.Equal("Valor", exception.ParamName);
        Assert.Contains("O valor precisa ser maior que zero", exception.Message);
    }
}
