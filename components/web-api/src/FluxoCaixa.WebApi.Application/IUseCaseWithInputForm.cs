namespace FluxoCaixa.WebApi.Application;

/// <summary>
/// Um caso de uso que requer entrada de dados
/// </summary>
/// <typeparam name="TForm">Tipo do dado de entrada</typeparam>
public interface IUseCaseWithInputForm<TForm>
{
    /// <summary>
    /// Executa o caso de uso
    /// </summary>
    /// <param name="form">Dados de entrada</param>
    Task ExecAsync(TForm form, CancellationToken cancellationToken);
}