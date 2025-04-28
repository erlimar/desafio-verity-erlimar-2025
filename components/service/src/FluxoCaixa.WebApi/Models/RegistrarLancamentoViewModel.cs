using System.ComponentModel.DataAnnotations;

namespace FluxoCaixa.WebApi.Models;

/// <summary>
/// Dados para registro de novo lançamento
/// </summary>
public class RegistrarLancamentoViewModel
{
    /// <summary>
    /// Data/Hora do lançamento (incluindo o fuso horário)
    /// </summary>
    [Required(ErrorMessage = "Você precisa informar uma data/hora")]
    public DateTime? DataHora { get; set; }

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
}