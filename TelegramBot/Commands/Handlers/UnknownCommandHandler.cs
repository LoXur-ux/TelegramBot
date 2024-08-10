namespace TelegramBot.Commands.Handlers;

/// <summary>
/// Обработчик неизвестных команд.
/// </summary>
public class UnknownCommandHandler
{
    public Task<string?> HandleUnknownCommandAsync()
        => Task.FromResult<string?>("Извините, но это неизвестная мне команда");
}
