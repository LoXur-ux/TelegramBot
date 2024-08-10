using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace TelegramBot.Handlers
{
    public class ErrorHandler
    {
        private readonly ILogger<ErrorHandler> _logger;
        private readonly ITelegramBotClient _client;

        public ErrorHandler(ILogger<ErrorHandler> logger, ITelegramBotClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<string?> HandleError(Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Ошибка!");

            //await NotifyAdminAsync(exception.Message, cancellationToken);

            return "Извините! Произошла ошибка!";
        }
    }
}
