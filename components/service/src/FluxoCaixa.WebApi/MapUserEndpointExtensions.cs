using System.Security.Claims;

namespace FluxoCaixa.WebApi;

/// <summary>
/// Extensão para mapear os endpoints relacionados ao usuário.
/// </summary>
public static class MapUserEndpointExtensions
{
    /// <summary>
    /// Mapeia o endpoint "users/me"
    /// </summary>
    public static IEndpointRouteBuilder MapUserMeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("users/me", (ClaimsPrincipal claimsPrincipal) =>
        {
            return claimsPrincipal.Claims.Select(c => new { c.Type, c.Value });
        })
        .WithSummary("Obtém o usuário atual")
        .WithDescription("Exibe as informações do usuário atual, incluindo o nome e o e-mail.")
        .WithTags("Users")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        // .Accepts(typeof(ClaimsPrincipal),"application/json" )
        .WithOpenApi()
        .RequireAuthorization();

        return endpoints;
    }
}