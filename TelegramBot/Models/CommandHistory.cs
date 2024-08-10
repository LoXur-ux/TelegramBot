using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Models
{
    public class CommandHistory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public long ChatId { get; set; }
        [Required]
        public string Command { get; set; }
        public string? Data {  get; set; }
        [Required]
        public DateTime? Created { get; set; }

    }
}
