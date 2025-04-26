namespace FluxoCaixa.Application.RegistrarConsolidado;

/// <summary>
/// Dados de entrada para caso de uso <see cref=""/>
/// </summary>
/// <param name="IdentificadorDono">Identificador do dono</param>
/// <param name="Data">Data do registro</param>
public record class RegistrarConsolidadoForm(string IdentificadorDono, DateTimeOffset Data);