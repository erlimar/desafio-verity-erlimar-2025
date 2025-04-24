namespace FluxoCaixa.Application.RegistrarLancamento;

/// <summary>
/// Quando o dono de um lançamento é inválido
/// </summary>
public class DonoLancamentoInvalidoException : Exception
{
    public DonoLancamentoInvalidoException(string identificadorDono)
    : base($"O dono de lançamento \"{identificadorDono}\" informado não é válido")
    {
    }
}