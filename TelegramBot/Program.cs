using System.Net.WebSockets;
using Telegram.Bot;
using Telegram.Bot.Types;
namespace TelegramBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new TelegramBotClient("7492270303:AAGuDO7cHOmQlVN_CQGXkKW-Z6BY33RioGk");
            client.StartReceiving(Update, Error);

            Console.ReadLine();
        }

        private static async Task Update(ITelegramBotClient client, Update update, CancellationToken token)
        {
            var message = update.Message;

            if (message == null)
            {
                return;
            }

            if (message.Text is string text && text.Contains("привет", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine($"{message.Chat.FirstName} ({message.Type}): {text}");
                await client.SendTextMessageAsync(message.Chat.Id, "Добрый день!");
                return;
            }
        }

        private static async Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
        { }
    }
}
