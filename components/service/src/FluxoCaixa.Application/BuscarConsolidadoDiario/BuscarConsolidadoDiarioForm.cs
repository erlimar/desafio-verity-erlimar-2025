namespace FluxoCaixa.Application.BuscarConsolidadoDiario;

/// <summary>
/// Dados de entrada para caso de uso <see cref="BuscarConsolidadoDiarioUseCase"/>
/// </summary>
/// <param name="IdentificadorDono">Identificador do dono</param>
/// <param name="Data">Data do consolidado</param>
public record class BuscarConsolidadoDiarioForm(
    string IdentificadorDono,
    DateTimeOffset Data);