using System;

using FluxoCaixa.Application.Models;
using FluxoCaixa.Application.Utils;

namespace FluxoCaixa.Application.RegistrarConsolidado;

/// <summary>
/// Caso de uso "Registrar consolidado", que registra uma entrada para um
/// cálculo de saldo consolidado
/// </summary>
public class RegistrarConsolidadoUseCase(IConsolidadoAppRepository consolidadoAppRepository)
    : IUseCaseInputOnly<RegistrarConsolidadoForm>
{
    private readonly IConsolidadoAppRepository _consolidadoAppRepository = consolidadoAppRepository
        ?? throw new ArgumentNullException(nameof(consolidadoAppRepository));

    /// <summary>
    /// Executa o caso de uso
    /// </summary>
    /// <param name="form">Dados de entrada</param>
    /// <exception cref="NotImplementedException">
    /// Quando o dono do lançamento não existe
    /// </exception>
    public async Task ExecAsync(RegistrarConsolidadoForm form, CancellationToken cancellationToken)
    {
        ValidateForm(form);

        form.Data.Deconstruct(out DateOnly dataOriginal, out _, out TimeSpan timeSpanOriginal);

        var data = new DateTimeOffset(dataOriginal, new TimeOnly(0), timeSpanOriginal);

        var resultado = await _consolidadoAppRepository.ObterConsolidadosDaDataAsync(
            form.IdentificadorDono,
            data,
            cancellationToken);

        if (resultado.Any())
        {
            throw new ConsolidadoJaExisteException();
        }

        await _consolidadoAppRepository.GravarConsolidadoAsync(new ConsolidadoModel()
        {
            Id = string.Empty,
            IdentificadorDono = form.IdentificadorDono,
            DataHora = data,
            Status = StatusConsolidado.Pendente,
            SaldoAnterior = 0,
            SaldoPeriodo = 0,
            SaldoFinal = 0
        }, cancellationToken);
    }

    private static void ValidateForm(RegistrarConsolidadoForm form)
    {
        _ = form ?? throw new ArgumentNullException(nameof(form));

        if (string.IsNullOrWhiteSpace(form.IdentificadorDono))
        {
            throw new ArgumentException(
                "O identificador do dono não pode ser vazio",
                nameof(form.IdentificadorDono));
        }
    }
}