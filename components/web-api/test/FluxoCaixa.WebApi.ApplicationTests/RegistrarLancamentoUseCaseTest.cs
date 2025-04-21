using FluxoCaixa.WebApi.Application.RegistrarLancamento;

using Moq;

namespace FluxoCaixa.WebApi.ApplicationTests;

[Trait("Target", nameof(RegistrarLancamentoUseCase))]
public class RegistrarLancamentoUseCaseTest
{
    [Fact(DisplayName = "O caso de uso \"Registrar Lançamento\" requer um [IIdentityProviderGateway]")]
    public void CasoDeUsoRequerUmGatewayDoIdentityProviderAsync()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new RegistrarLancamentoUseCase(null!));

        Assert.Equal("identityProviderGateway", exception.ParamName);
    }

    [Fact(DisplayName = "O caso de uso \"Registrar Lançamento\" requer dados de entrada")]
    public async Task CasoDeUsoRequerDadosDeEntradaAsync()
    {
        var identityProviderGateway = new Mock<IIdentityProviderGateway>().Object;
        var useCase = new RegistrarLancamentoUseCase(identityProviderGateway);
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => useCase.ExecAsync(null!));

        Assert.Equal("form", exception.ParamName);
    }

    [Theory(DisplayName = "O caso de uso \"Registrar Lançamento\" requer identificador do dono")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task CasoDeUsoRequerIdentificadorDeDonoValidoAsync(string? identificadorInvalido)
    {
        var identityProviderGateway = new Mock<IIdentityProviderGateway>().Object;
        var useCase = new RegistrarLancamentoUseCase(identityProviderGateway);
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
        var identityProviderGateway = new Mock<IIdentityProviderGateway>().Object;
        var useCase = new RegistrarLancamentoUseCase(identityProviderGateway);
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
        var identityProviderGateway = new Mock<IIdentityProviderGateway>().Object;
        var useCase = new RegistrarLancamentoUseCase(identityProviderGateway);
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => useCase.ExecAsync(new("Dono",
                TipoLancamento.Credito,
                DateTimeOffset.UtcNow,
                valorInvalido,
                "Descricao")));

        Assert.Equal("Valor", exception.ParamName);
        Assert.Contains("O valor precisa ser maior que zero", exception.Message);
    }

    [Fact(DisplayName = "O dono deve existir")]
    public async Task ODonoDeveExistir()
    {
        var identityProviderMock = new Mock<IIdentityProviderGateway>();
        var useCase = new RegistrarLancamentoUseCase(identityProviderMock.Object);
        var exception = await Assert.ThrowsAsync<DonoLancamentoInvalidoException>(
            () => useCase.ExecAsync(new("codigo-nao-existente",
                TipoLancamento.Credito,
                DateTimeOffset.UtcNow,
                1,
                "Descricao")));

        Assert.Equal("O dono de lançamento \"codigo-nao-existente\" informado não é válido", exception.Message);

        identityProviderMock.Verify(m => m.UsuarioExiste("codigo-nao-existente"), Times.Once);
    }
}