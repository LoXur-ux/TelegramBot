
namespace TelegramBot.Commands
{
    public class StartCommandHandler : ICommandHandler
    {
        public string Command => "/start";

        public async Task<string> HandleAsync(string data = "")
        {
            return "Start";
        }
    }
}
