namespace TelegramBot.Commands
{
    public class HelpCommandHandler : ICommandHandler
    {
        public string Command => "/help";

        public async Task<string?> HandleAsync(string data = "")
        {
            var result = "/start – Начало общения с ботом.\n" +
                "/help – Вывод справку о доступных командах.\n" +
                "/hello – Вывод имени и фамилии разработчика, email и ссылку на github.\n" +
                "/inn – Получить наименования и адреса компаний по ИНН. Если их несколько, вы можете записать их через пробел.\n" +
                "Структура команды: /inn [ИНН]\n" +
                "Пример: /inn 7731457980 77094209\n" +
                "/last – Повторить последнее действие бота.";

            return result;
        }
    }
}
