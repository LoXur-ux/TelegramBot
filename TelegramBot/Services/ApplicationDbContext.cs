using Microsoft.EntityFrameworkCore;
using TelegramBot.Models;

namespace TelegramBot.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CommandHistory> CommandHistories { get; set; }
    }
}
