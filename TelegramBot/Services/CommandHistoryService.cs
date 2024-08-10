using Microsoft.EntityFrameworkCore;
using TelegramBot.Models;

namespace TelegramBot.Services
{
    public class CommandHistoryService
    {
        private readonly ApplicationDbContext _context;

        public CommandHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveCommandAsync(long chatId, string command, string? data = null)
        {
            try
            {
                var commandHistory = new CommandHistory()
                {
                    ChatId = chatId,
                    Command = command,
                    Created = DateTime.Now.ToUniversalTime()
                };


                if (data != null)
                {
                    commandHistory.Data = data;
                }

                await _context.CommandHistories.AddAsync(commandHistory);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { }
            
        }

        public async Task<CommandHistory?> GetLastCommandAsync(long chatId)
        {
            try
            {
                return await _context.CommandHistories
                .Where(x => x.ChatId == chatId)
                .OrderByDescending(x => x.Created)
                .FirstOrDefaultAsync();
            }
            catch (Exception ex) { return null; }
            
        }
    }
}
