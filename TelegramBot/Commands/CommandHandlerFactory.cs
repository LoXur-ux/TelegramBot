using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Commands.Handlers;

namespace TelegramBot.Commands
{
    public class CommandHandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string?> HandleCommandAsync(string command, string data, long chatId)
        {
            try
            {
                var handlers = _serviceProvider.GetServices<ICommandHandler>();
                var handler = handlers.FirstOrDefault(x => x.Command.Equals(command, StringComparison.OrdinalIgnoreCase));
                if (handler == null)
                {
                    return await _serviceProvider.GetRequiredService<UnknownCommandHandler>().HandleUnknownCommandAsync();
                }

                handler.Data = data;
                return await handler.HandleAsync(chatId);
            }
            catch (Exception ex) { return string.Empty; }

        }
    }
}
