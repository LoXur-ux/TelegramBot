
namespace TelegramBot.Commands
{
    public class StartCommandHandler : ICommandHandler
    {
        public string Command => "/start";

        public async Task<string> HandleAsync()
        {
            return "Start";
        }

        public Task Log()
        {
            throw new NotImplementedException();
        }
    }
}
