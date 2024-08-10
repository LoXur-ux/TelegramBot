using Microsoft.EntityFrameworkCore;
using TelegramBot.Models;

namespace TelegramBot.Services;

public class CommandHistoryService
{
    /// <summary>
    /// Контекст БД.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="context">Контекст БД.</param>
    public CommandHistoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Сохранение выполненой команды в БД.
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="command">Команда.</param>
    /// <param name="data">Данные для команды.</param>
    public async Task SaveCommandAsync(long chatId, string command, string? data = null)
    {
        try
        {
            // Создание экземпляра.
            var commandHistory = new CommandHistory()
            {
                ChatId = chatId,
                Command = command,
                Created = DateTime.Now.ToUniversalTime(),
                Data = data
            };

            // Добавление экземпляра в контекст.
            await _context.CommandHistories.AddAsync(commandHistory);
            // Обновление БД.
            await _context.SaveChangesAsync();
        }
        catch (Exception ex) { }

    }

    /// <summary>
    /// Получение последней выполненной команды.
    /// </summary>
    /// <param name="chatId">Идентификатор чата, по которому ищется команда.</param>
    /// <returns>Экземпляр выполненной команды.</returns>
    public async Task<CommandHistory?> GetLastCommandAsync(long chatId)
    {
        try
        {
            return await _context.CommandHistories.Where(x => x.ChatId == chatId)
                .OrderByDescending(x => x.Created)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex) { return null; }

    }
}
