using Microsoft.Extensions.Logging;

namespace TelegramBot.Handlers;

/// <summary>
/// Класс-обработчик ошибок.
/// </summary>
public class ErrorHandler
{
    /// <summary>
    /// Логер.
    /// </summary>
    private readonly ILogger<ErrorHandler> _logger;

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="logger">Логер.</param>
    public ErrorHandler(ILogger<ErrorHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Обработчик ошибок.
    /// </summary>
    /// <param name="exception">Ошибка.</param>
    /// <param name="cancellationToken">Токен прекращения работы.</param>
    /// <returns>Ответ для пользователя.</returns>
    public async Task<string?> HandleError(Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Ошибка!");

        return "Извините! Произошла ошибка!";
    }
}
