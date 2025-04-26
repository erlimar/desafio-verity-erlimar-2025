namespace FluxoCaixa.Application.RegistrarConsolidado;

/// <summary>
/// Quando se tenta cadastrar um consolidado em uma data onde já existe um
/// cadastro
/// </summary>
public class ConsolidadoJaExisteException : Exception
{
    public ConsolidadoJaExisteException()
        : base("Já existe um consolidado para a data informada")
    { }
}