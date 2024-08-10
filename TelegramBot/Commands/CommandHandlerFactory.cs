namespace TelegramBot.Commands
{
    public class CommandHandlerFactory
    {
        private readonly IEnumerable<ICommandHandler> _handlers;
        private readonly UnknownCommandHandler _unknownCommandHandler;

        public CommandHandlerFactory(IEnumerable<ICommandHandler> handlers, UnknownCommandHandler unknownCommandHandler)
        {
            _handlers = handlers;
            _unknownCommandHandler = unknownCommandHandler;
        }

        public async Task<string?> HandleCommandAsync(string command)
        {
            var handler = _handlers.FirstOrDefault(x => x.Command == command);

            return handler != null
                ? await handler.HandleAsync()
                : await _unknownCommandHandler.HandleUnknownCommandAsync();
        }
    }
}
