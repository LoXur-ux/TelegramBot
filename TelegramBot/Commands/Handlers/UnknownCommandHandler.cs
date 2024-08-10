namespace TelegramBot.Commands.Handlers
{
    public class UnknownCommandHandler
    {
        public Task<string?> HandleUnknownCommandAsync()
            => Task.FromResult<string?>("Извините, но это неизвестная мне команда");
    }
}
