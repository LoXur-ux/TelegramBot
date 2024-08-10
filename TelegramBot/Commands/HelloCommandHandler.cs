namespace TelegramBot.Commands
{
    public class HelloCommandHandler : ICommandHandler
    {
        public string Command => "/hello";

        public async Task<string?> HandleAsync()
        {
            return "Hello World!";
        }
    }
}
