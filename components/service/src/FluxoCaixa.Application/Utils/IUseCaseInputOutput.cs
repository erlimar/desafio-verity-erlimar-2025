namespace FluxoCaixa.Application.Utils;

/// <summary>
/// Um caso de uso que requer entrada de dados e retorna resultado
/// </summary>
/// <typeparam name="TForm">Tipo do dado de entrada</typeparam>
public interface IUseCaseInputOutput<TForm, TOutput>
{
    /// <summary>
    /// Executa o caso de uso
    /// </summary>
    /// <param name="form">Dados de entrada</param>
    /// <returns><see cref="TOutput"/></returns>
    Task<TOutput> ExecAsync(TForm form, CancellationToken cancellationToken);
}