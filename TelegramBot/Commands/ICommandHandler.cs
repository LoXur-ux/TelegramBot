using TelegramBot.Services;

namespace TelegramBot.Commands;

/// <summary>
/// Интерфейс принимаемых от пользователя команд.
/// </summary>
public interface ICommandHandler
{
    /// <summary>
    /// Команда.
    /// </summary>
    string Command {  get; }
    /// <summary>
    /// Данные для команды.
    /// Необязательны.
    /// </summary>
    string? Data { get; set; }
    /// <summary>
    /// Сервис взаимодействия с базой данных.
    /// </summary>
    CommandHistoryService CommandHistoryService { get; set; }
    /// <summary>
    /// Метод выполнения команды, запрошенной пользователем.
    /// </summary>
    /// <param name="chatId">Идентификатор чата. Нужен для хранения истории.</param>
    /// <returns>Сообщение для пользователя</returns>
    Task<string> HandleAsync(long chatId);
    /// <summary>
    /// Метод для сохранения запроса пользователя в БД.
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    Task SaveCommandInHistory(long chatId);
}
