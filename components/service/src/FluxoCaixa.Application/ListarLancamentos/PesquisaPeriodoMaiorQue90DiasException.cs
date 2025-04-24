namespace FluxoCaixa.Application.ListarLancamentos;

/// <summary>
/// Quando se tenta pesquisar lançamentos em um período maior que 90 dias
/// </summary>
public class PesquisaPeriodoMaiorQue90DiasException : Exception
{
    public PesquisaPeriodoMaiorQue90DiasException()
        : base("Você só pode pesquisar lançamentos em um período de até 90 dias")
    {
    }
}
