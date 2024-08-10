using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Commands;
using TelegramBot.Handlers;

namespace TelegramBot.Services;

/// <summary>
/// Хост телеграм бота.
/// Реализует IHostedService.
/// </summary>
public class TelegramBotHostedServices : IHostedService
{
    /// <summary>
    /// Клиент телеграм бота.
    /// </summary>
    private readonly ITelegramBotClient _client;
    /// <summary>
    /// Фабрика команд.
    /// </summary>
    private readonly CommandHandlerFactory _commandHandlerFactory;
    /// <summary>
    /// Обработчик ошибок.
    /// </summary>
    private readonly ErrorHandler _errorHandler;

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="client">Клиент телеграм бота.</param>
    /// <param name="commandHandlerFactory">Фабрика команд.</param>
    /// <param name="errorHandler">Обработчик ошибок.</param>
    public TelegramBotHostedServices(ITelegramBotClient client, CommandHandlerFactory commandHandlerFactory, ErrorHandler errorHandler)
    {
        _client = client;
        _commandHandlerFactory = commandHandlerFactory;
        _errorHandler = errorHandler;
    }

    /// <summary>
    /// Старт хоста.
    /// </summary>
    /// <param name="cancellationToken">Токен прерывания работы.</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var reciverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        // Запуск прослушивания чатов.
        _client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, reciverOptions, cancellationToken);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Метод обработки получения сообщений.
    /// </summary>
    /// <param name="client">Клиент телеграм бота.</param>
    /// <param name="update">Полученное обновление (сообщение).</param>
    /// <param name="cancellationToken">Токен прерывания работы.</param>
    /// <returns></returns>
    private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not Message message
            || message.Text is not string messageText)
        {
            return;
        }

        // Подготовка комманды.
        messageText = messageText.ToLower();
        var command = messageText.Split(' ').First();
        var data = messageText.Replace(command, string.Empty);

        // Вызов фабрики для выполнения обработки полученной комады.
        var response = await _commandHandlerFactory.HandleCommandAsync(command, data, message.Chat.Id);

        // Отправка результата работы пользователю.
        await client.SendTextMessageAsync(message.Chat.Id,
            string.IsNullOrWhiteSpace(response) ? "К сожалению, требуемой информации не нашлось." : response,
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Обработчик ошибок.
    /// </summary>
    /// <param name="client">Клиент телеграм бота.</param>
    /// <param name="exception">Ошибка.</param>
    /// <param name="cancellationToken">Токен прерывания работы.</param>
    /// <returns>Ответ пользователю.</returns>
    private async Task<string?> HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        return await _errorHandler.HandleError(exception, cancellationToken);
    }

    /// <summary>
    /// Прекрщение работы хоста.
    /// </summary>
    /// <param name="cancellationToken"></param>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
