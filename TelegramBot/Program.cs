using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramBot.Commands;
using TelegramBot.Configurations;
using TelegramBot.Handlers;
using TelegramBot.Services;

namespace TelegramBot
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", false, true);
                    config.AddUserSecrets<Program>();
                })
                .ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;

                    services.Configure<TelegramBotOptions>(configuration.GetSection("TelegramBot"));
                    //services.Configure<ApiOptions>(configuration.GetSection("ApiOptions"));

                    services.AddSingleton<ITelegramBotClient>(provider =>
                    {
                        var option = provider.GetRequiredService<IOptions<TelegramBotOptions>>();
                        return new TelegramBotClient(option.Value.Token);
                    });

                    services.AddSingleton<ICommandHandler, StartCommandHandler>();
                    services.AddSingleton<ICommandHandler, HelloCommandHandler>();
                    services.AddSingleton<ICommandHandler, HelpCommandHandler>();
                    services.AddSingleton<ICommandHandler, INNCommandHandler>();
                    services.AddSingleton<CommandHandlerFactory>();
                    services.AddSingleton<UnknownCommandHandler>();
                    services.AddSingleton<ErrorHandler>(provider =>
                    {
                        var logger = provider.GetRequiredService<ILogger<ErrorHandler>>();
                        var client = provider.GetRequiredService<ITelegramBotClient>();
                        var configuration = provider.GetRequiredService<IConfiguration>();
                        return new ErrorHandler(logger, client);
                    });

                    services.AddHostedService<TelegramBotHostedServices>();
                })
                .Build();

            await host.StartAsync();

            Console.ReadLine();
        }
    }
}
