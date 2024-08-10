
namespace TelegramBot.Commands
{
    public class INNCommandHandler : ICommandHandler
    {
        public string Command => "/inn";

        public async Task<string> HandleAsync()
        {
            return "inn";
        }
    }
}
