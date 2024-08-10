using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Commands.Handlers;

namespace TelegramBot.Commands;

/// <summary>
/// Фабрика определяет какой обработчик необходимо вызвать.
/// </summary>
public class CommandHandlerFactory
{
    /// <summary>
    /// Сервис провайдер. Хранит информацию о зарегистрированных сервисах.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    public CommandHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Обработчик вызова команд.
    /// </summary>
    /// <param name="command">Команда.</param>
    /// <param name="data">Данные.</param>
    /// <param name="chatId">Идентифиактор чата.</param>
    /// <returns>Строка-ответ пользователю.</returns>
    public async Task<string?> HandleCommandAsync(string command, string data, long chatId)
    {
        try
        {
            var handlers = _serviceProvider.GetServices<ICommandHandler>();
            var handler = handlers.FirstOrDefault(x => x.Command.Equals(command, StringComparison.OrdinalIgnoreCase));
            if (handler == null)
            {
                return await _serviceProvider.GetRequiredService<UnknownCommandHandler>().HandleUnknownCommandAsync();
            }

            handler.Data = data;
            return await handler.HandleAsync(chatId);
        }
        catch (Exception ex) { return string.Empty; }

    }
}
