using TelegramBot.Services;

namespace TelegramBot.Commands
{
    public interface ICommandHandler
    {
        string Command {  get; }
        string? Data { get; set; }
        CommandHistoryService CommandHistoryService { get; set; }
        Task<string> HandleAsync(long chatId);
        Task SaveCommandInHistory(long chatId);
    }
}
