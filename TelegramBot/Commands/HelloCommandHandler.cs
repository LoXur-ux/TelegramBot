namespace TelegramBot.Commands
{
    public class HelloCommandHandler : ICommandHandler
    {
        public string Command => "/hello";

        public async Task<string?> HandleAsync(string data = "")
        {
            var result = "Данил Панишев\n" +
                "Email: starkaw@yandex.ru\n" +
                "github: https://github.com/LoXur-ux/TelegramBot";
            return result;
        }
    }
}
