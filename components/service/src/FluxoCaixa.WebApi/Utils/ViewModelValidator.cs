using System.ComponentModel.DataAnnotations;

namespace FluxoCaixa.WebApi.Utils;

/// <summary>
/// Classe de validação de ViewModels
/// </summary>
public static class ViewModelValidator
{
    /// <summary>
    /// Verifica se o ViewModel é válido de acordo com suas anotações
    /// </summary>
    /// <typeparam name="TViewModel">Tipo do modelo</typeparam>
    /// <param name="model">Instância do modelo</param>
    /// <param name="validationResult">Quando não é válido, contém as falhas</param>
    /// <returns>True se for válido, e False se não</returns>
    public static bool ViewModelIsValid<TViewModel>(TViewModel model, out IEnumerable<ValidationResult> validationResult)
        where TViewModel : class
    {
        var validationContext = new ValidationContext(model);
        var temporaryValidationResult = new List<ValidationResult>();

        if (!Validator.TryValidateObject(model, validationContext, temporaryValidationResult, true))
        {
            validationResult = temporaryValidationResult;
            return false;
        }

        validationResult = Enumerable.Empty<ValidationResult>();
        return true;
    }
}