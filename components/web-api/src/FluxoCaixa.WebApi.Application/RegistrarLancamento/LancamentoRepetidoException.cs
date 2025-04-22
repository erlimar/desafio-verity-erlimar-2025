namespace FluxoCaixa.WebApi.Application.RegistrarLancamento;

public class LancamentoRepetidoException : Exception
{
    public LancamentoRepetidoException()
        : base("Este lançamento já parece ter sido feito antes")
    {
    }
}