using TelegramBot.Services;

namespace TelegramBot.Commands.Handlers;

public class HelpCommandHandler : ICommandHandler
{
    public string Command => "/help";
    public string? Data { get; set; }

    public CommandHistoryService CommandHistoryService { get; set; }

    public HelpCommandHandler(CommandHistoryService commandHistoryService)
    {
        CommandHistoryService = commandHistoryService;
    }

    public async Task<string?> HandleAsync(long chatId)
    {
        await SaveCommandInHistory(chatId);

        var result = "/start – Начало общения с ботом.\n" +
            "/help – Вывод справку о доступных командах.\n" +
            "/hello – Вывод имени и фамилии разработчика, email и ссылку на github.\n" +
            "/inn – Получить наименования и адреса компаний по ИНН. Если их несколько, вы можете записать их через пробел.\n" +
            "\tСтруктура команды: /inn ИНН [ИНН]\n" +
            "\tПример: /inn 7731457980 77094209\n" +
            "/okved - Выводит по ИНН коды ОКВЭД и виды деятельности компании, отсортированные в обратном алфавитном порядке по виду деятельности (к сожалению, есть ограничения по количеству получаемых кодов до одного).\n" +
            "\tСтруктура команды: /okved ИНН\n" +
            "\tПример: /okved 7731457980\n" +
            "/last – Повторить последнее действие бота.";

        return result;
    }

    public async Task SaveCommandInHistory(long chatId)
    {
        await CommandHistoryService.SaveCommandAsync(chatId, Command);
    }
}
