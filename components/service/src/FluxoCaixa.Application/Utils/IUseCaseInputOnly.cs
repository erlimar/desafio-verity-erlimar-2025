namespace FluxoCaixa.Application.Utils;

/// <summary>
/// Um caso de uso que requer entrada de dados e não retorna resultado
/// </summary>
/// <typeparam name="TForm">Tipo do dado de entrada</typeparam>
public interface IUseCaseInputOnly<TForm>
{
    /// <summary>
    /// Executa o caso de uso
    /// </summary>
    /// <param name="form">Dados de entrada</param>
    Task ExecAsync(TForm form, CancellationToken cancellationToken);
}