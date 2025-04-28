using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using FluxoCaixa.Application.RegistrarLancamento;
using FluxoCaixa.WebApi.Extensions;
using FluxoCaixa.WebApi.Models;

using static FluxoCaixa.WebApi.Utils.ViewModelValidator;

namespace FluxoCaixa.WebApi;

/// <summary>
/// Extensão para mapear os endpoints relacionados a lançamentos
/// </summary>
public static class MapLancamentoEndpoints
{
    /// <summary>
    /// Mapeia o endpoint para registro de lançamento
    /// </summary>
    public static RouteGroupBuilder MapRegistrarLancamento(this RouteGroupBuilder group)
    {
        _ = group.MapPost("/", async (
            ClaimsPrincipal principal,
            RegistrarLancamentoUseCase registrarLancamentoUseCase,
            RegistrarLancamentoViewModel model,
            CancellationToken cancellationToken) =>
        {
            if (!ViewModelIsValid(model, out IEnumerable<ValidationResult> validationResult))
            {
                return Results.BadRequest(validationResult);
            }

            try
            {
                await registrarLancamentoUseCase.ExecAsync(
                    model.ToUseCaseForm(principal.GetAuthenticatedUserId()),
                    cancellationToken);
            }
            catch (Exception e) when (e is ArgumentNullException or ArgumentException or DonoLancamentoInvalidoException or LancamentoRepetidoException)
            {
                return Results.BadRequest(new List<ValidationResult>
                {
                    new(e.Message)
                });
            }

            return Results.Created();
        })
        .WithSummary("Registrar lançamento")
        .WithDescription("""Executa o caso de uso "Registrar Lançamento", que cadastra um novo lançamento""")
        .Accepts(typeof(RegistrarLancamentoViewModel), "application/json")
        .ProducesValidationProblem(StatusCodes.Status400BadRequest, "application/json")
        .Produces(StatusCodes.Status201Created)
        .WithOpenApi()
        .RequireAuthorization();

        return group;
    }
}