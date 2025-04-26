namespace FluxoCaixa.Application.RegistrarLancamento;

/// <summary>
/// Dados de entrada para caso de uso <see cref="RegistrarLancamentoUseCase"/>
/// </summary>
/// <param name="IdentificadorDono">Identificador do dono</param>
/// <param name="Tipo">Tipo do lançamento</param>
/// <param name="DataHora">Data/Hora do lançamento</param>
/// <param name="Valor">Valor do lançamento</param>
/// <param name="Descricao">Descrição do lançamento</param>
public record RegistrarLancamentoForm(
    string IdentificadorDono,
    TipoLancamento Tipo,
    DateTimeOffset DataHora,
    decimal Valor,
    string Descricao);