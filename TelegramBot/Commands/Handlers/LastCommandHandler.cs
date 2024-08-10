using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Services;

namespace TelegramBot.Commands.Handlers
{
    public class LastCommandHandler : ICommandHandler
    {
        public string Command => "/last";
        public string? Data { get; set; }
        public CommandHistoryService CommandHistoryService { get; set; }

        private readonly IServiceProvider _serviceProvider;
        private readonly CommandHistoryService _commandHystoryService;

        public LastCommandHandler(IServiceProvider serviceProvider, CommandHistoryService commandHystoryService)
        {
            _serviceProvider = serviceProvider;
            _commandHystoryService = commandHystoryService;
        }

        public async Task<string?> HandleAsync(long chatId)
        {
            var commandHistory = await _commandHystoryService.GetLastCommandAsync(chatId);
            if (commandHistory == null)
            {
                return "Произошла ошабка! Прошлая команда неизвестна!";
            }

            var factory = _serviceProvider.GetRequiredService<CommandHandlerFactory>();
            var result = await factory.HandleCommandAsync(commandHistory.Command, commandHistory.Data, commandHistory.ChatId);
            return $"Ваша последняя команда: {commandHistory.Command} ({commandHistory.Created?.ToString("g")}):\n{result}";
        }

        public async Task SaveCommandInHistory(long chatId)
        {
            await CommandHistoryService.SaveCommandAsync(chatId, Command, null);
        }
    }
}
