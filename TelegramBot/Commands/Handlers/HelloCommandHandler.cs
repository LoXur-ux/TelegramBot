using TelegramBot.Services;

namespace TelegramBot.Commands.Handlers
{
    public class HelloCommandHandler : ICommandHandler
    {
        public string Command => "/hello";
        public string? Data { get; set; }
        public CommandHistoryService CommandHistoryService { get; set; }

        public HelloCommandHandler(CommandHistoryService commandHistoryService)
        {
            CommandHistoryService = commandHistoryService;
        }

        public async Task<string?> HandleAsync(long chatId)
        {
            await SaveCommandInHistory(chatId);

            var result = "Разработчик: Данил Панишев\n" +
                "Email: starkaw@yandex.ru\n" +
                "github: https://github.com/LoXur-ux/TelegramBot";
            return result;
        }

        public async Task SaveCommandInHistory(long chatId)
        {
            await CommandHistoryService.SaveCommandAsync(chatId, Command);
        }
    }
}
