namespace FluxoCaixa.Application.ListarLancamentos;

/// <summary>
/// Dados de entrada para caso de uso <see cref="ListarLancamentosUseCase"/>
/// </summary>
/// <param name="IdentificadorDono">Identificador do dono</param>
/// <param name="PeriodoInicial">Data inicial do período</param>
/// <param name="PeriodoFinal">Data limite do período</param>
/// <param name="Offset">Posição no resultado à considerar para paginação</param>
public record class ListarLancamentosForm(string IdentificadorDono,
    DateTimeOffset PeriodoInicial,
    DateTimeOffset PeriodoFinal,
    int Offset);