namespace TelegramBot.Commands
{
    public class HelpCommandHandler : ICommandHandler
    {
        public string Command => "/help";

        public async Task<string?> HandleAsync()
        {
            return "Help me!";
        }

        public Task Log()
        {
            throw new NotImplementedException();
        }
    }
}
