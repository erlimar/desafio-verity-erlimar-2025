using System.ComponentModel.DataAnnotations;

using FluxoCaixa.Application.Models;
using FluxoCaixa.Application.RegistrarLancamento;

namespace FluxoCaixa.WebApi.Models;

/// <summary>
/// Dados para registro de novo lançamento
/// </summary>
public class RegistrarLancamentoViewModel : IValidatableObject
{
    /// <summary>
    /// Data/Hora do lançamento (incluindo o fuso horário)
    /// </summary>
    [Required(ErrorMessage = "Você precisa informar uma data/hora")]
    public DateTimeOffset? DataHora { get; set; }

    /// <summary>
    /// Valor do lançamento
    /// </summary>
    [Required(ErrorMessage = "Você precisa informar um valor")]
    public decimal? Valor { get; set; }

    /// <summary>
    /// Descrição do lançamento
    /// </summary>
    [Required(ErrorMessage = "Você precisa informar uma descrição")]
    public string? Descricao { get; set; }

    /// <summary>
    /// Converte o modelo de ViewModel para o modelo de caso de uso
    /// </summary>
    /// <param name="idUsuario">
    /// Id do usuário autenticado para se tornar dono do lançamento
    /// </param>
    public RegistrarLancamentoForm ToUseCaseForm(string idUsuario)
    {
        TipoLancamento tipoLancamento = Valor < 0m
            ? TipoLancamento.Debito
            : TipoLancamento.Credito;

        // Se o valor for negativo, transforma em positivo porque
        // o tipo de lançamento já está definido
        if (Valor < 0m)
        {
            Valor *= -1;
        }

#pragma warning disable CS8629 // Nullable value type may be null.
        return new RegistrarLancamentoForm(
            idUsuario,
            tipoLancamento,
            DataHora.Value,
            Valor.Value,
            Descricao!);
#pragma warning restore CS8629 // Nullable value type may be null.
    }

    /// <summary>
    /// Validações extras para o modelo de ViewModel
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (Valor == 0m)
        {
            results.Add(new ValidationResult(
                "Valor não pode ser zero. Use um valor negativo ou maior que zero.",
                [nameof(Valor)]));
        }

        if (string.IsNullOrWhiteSpace(Descricao))
        {
            results.Add(new ValidationResult(
                "Descrição não pode ser vazio, ou com espaços em branco",
                [nameof(Descricao)]));
        }

        return results;
    }
}