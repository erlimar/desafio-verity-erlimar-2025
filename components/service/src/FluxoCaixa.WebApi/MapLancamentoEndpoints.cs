using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using FluxoCaixa.WebApi.Models;

using static FluxoCaixa.WebApi.ViewModelValidator;

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
        _ = group.MapPost("/", (ClaimsPrincipal principal, RegistrarLancamentoViewModel model) =>
        {
            if (!ViewModelIsValid(model, out IEnumerable<ValidationResult> validationResult))
            {
                return Results.BadRequest(validationResult);
            }

            return Results.Ok(principal.Claims.Select(c => new { c.Type, c.Value }));
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