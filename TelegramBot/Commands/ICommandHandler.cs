namespace TelegramBot.Commands
{
    public interface ICommandHandler
    {
        string Command {  get; }
        Task<string> HandleAsync();

        Task Log();
    }
}
