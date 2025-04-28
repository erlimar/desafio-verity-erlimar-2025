using System.Security.Claims;

namespace FluxoCaixa.WebApi;

/// <summary>
/// Extensões para <see cref="ClaimsPrincipal"/>
/// </summary>
public static class ClaimsPrincipalExtensons
{
    /// <summary>
    /// Extrai o ID do usuário autenticado a partir do <see cref="ClaimsPrincipal"/>
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Quando o usuário não está devidamente autenticado
    /// </exception>
    public static string GetAuthenticatedUserId(this ClaimsPrincipal principal)
    {
        var userIdClaim = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("Usuário não autenticado");

        return userIdClaim.Value;
    }
}