using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Models;

/// <summary>
/// Модель данных. Предназначена для хранения истории выполненных команд.
/// </summary>
public class CommandHistory
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Идентификатор чата.
    /// </summary>
    [Required]
    public long ChatId { get; set; }
    /// <summary>
    /// Комманда, которая была выполнена.
    /// </summary>
    [Required]
    public string Command { get; set; }
    /// <summary>
    /// Данные для команды, которые требовались для выполнения.
    /// </summary>
    public string? Data {  get; set; }
    /// <summary>
    /// Дата и время выполнения команды.
    /// </summary>
    [Required]
    public DateTime? Created { get; set; }

}
