using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Services;

namespace TelegramBot.Commands.Handlers;

public class LastCommandHandler : ICommandHandler
{
    public string Command => "/last";
    public string? Data { get; set; }
    public CommandHistoryService CommandHistoryService { get; set; }

    /// <summary>
    /// Профайдер сервисов. Хранит в себе список всех зарегистрированных сервисов.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="serviceProvider">Сервис провайдер, хранящий список зарегистрированных сервисов.</param>
    /// <param name="commandHystoryService">Экземпляр класса для взаимодействия с БД.</param>
    public LastCommandHandler(IServiceProvider serviceProvider, CommandHistoryService commandHystoryService)
    {
        _serviceProvider = serviceProvider;
        CommandHistoryService = commandHystoryService;
    }

    public async Task<string?> HandleAsync(long chatId)
    {
        var commandHistory = await CommandHistoryService.GetLastCommandAsync(chatId);
        if (commandHistory == null)
        {
            return "Произошла ошабка! Прошлая команда неизвестна!";
        }

        var factory = _serviceProvider.GetRequiredService<CommandHandlerFactory>();
        var result = await factory.HandleCommandAsync(commandHistory.Command, commandHistory.Data, commandHistory.ChatId);
        return $"Ваша последняя команда: \"{commandHistory.Command} {commandHistory.Data}\" ({commandHistory.Created?.ToString("g")}):\n{result}";
    }

    public async Task SaveCommandInHistory(long chatId)
    {
        await CommandHistoryService.SaveCommandAsync(chatId, Command, null);
    }
}
