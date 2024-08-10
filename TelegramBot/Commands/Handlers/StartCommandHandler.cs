using TelegramBot.Services;

namespace TelegramBot.Commands.Handlers;

public class StartCommandHandler : ICommandHandler
{
    public string Command => "/start";
    public string? Data { get; set; }
    public CommandHistoryService CommandHistoryService { get; set; }

    public StartCommandHandler(CommandHistoryService commandHistoryService)
    {
        CommandHistoryService = commandHistoryService;
    }

    public async Task<string> HandleAsync(long chatId)
    {
        await SaveCommandInHistory(chatId);

        return "Привет! Давай начнем!";
    }

    public async Task SaveCommandInHistory(long chatId)
    {
        await CommandHistoryService.SaveCommandAsync(chatId, Command, null);
    }
}
