using Dadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramBot.Commands;
using TelegramBot.Commands.Handlers;
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
                // Получения данных из конфигурационного файла.
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", false, true);
                    config.AddUserSecrets<Program>();
                })
                .ConfigureServices((context, services) =>
                {
                    // Добавление конфигурации.
                    var configuration = context.Configuration;
                    services.Configure<TelegramBotOptions>(configuration.GetSection("TelegramBot"));
                    services.Configure<DadataOptions>(configuration.GetSection("DaData"));
                    services.Configure<SQLOptions>(configuration.GetSection("SQL"));

                    services.AddSingleton<ITelegramBotClient>(provider =>
                    {
                        var option = provider.GetRequiredService<IOptions<TelegramBotOptions>>().Value;
                        return new TelegramBotClient(option.Token);
                    });

                    services.AddDbContext<ApplicationDbContext>((provider, options) =>
                    {
                        var connectionString = provider.GetRequiredService<IOptions<SQLOptions>>().Value;
                        options.UseNpgsql(connectionString.ConnectionString);
                    });

                    services.AddSingleton<ISuggestClientAsync>(provider =>
                    {
                        var option = provider.GetRequiredService<IOptions<DadataOptions>>().Value;
                        return new SuggestClientAsync(option.Token);
                    });

                    services.AddSingleton<IOutwardClientAsync>(provider =>
                    {
                        var option = provider.GetRequiredService<IOptions<DadataOptions>>().Value;
                        return new OutwardClientAsync(option.Token);
                    });

                    services.AddSingleton<ICommandHandler, StartCommandHandler>();
                    services.AddSingleton<ICommandHandler, HelloCommandHandler>();
                    services.AddSingleton<ICommandHandler, HelpCommandHandler>();
                    services.AddSingleton<ICommandHandler, INNCommandHandler>();
                    services.AddSingleton<ICommandHandler, LastCommandHandler>();
                    services.AddSingleton<ICommandHandler, OkvedCommandHandler>();
                    services.AddSingleton<CommandHistoryService>();
                    services.AddSingleton<UnknownCommandHandler>();
                    services.AddSingleton<CommandHandlerFactory>();
                    services.AddSingleton<ErrorHandler>(provider =>
                    {
                        var logger = provider.GetRequiredService<ILogger<ErrorHandler>>();
                        var client = provider.GetRequiredService<ITelegramBotClient>();
                        return new ErrorHandler(logger, client);
                    });

                    // Сервис, который должен выполняться хостом.
                    services.AddHostedService<TelegramBotHostedServices>();
                })
                .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex) { }
            }

            await host.StartAsync();

            Console.ReadLine();
        }
    }
}
