using Microsoft.EntityFrameworkCore;
using TelegramBot.Models;

namespace TelegramBot.Services;

/// <summary>
/// Класс контекста БД;
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    /// <summary>
    /// Сущность истории выполненных команд для БД.
    /// </summary>
    public DbSet<CommandHistory> CommandHistories { get; set; }
}
