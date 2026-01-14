using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramBotApp.Data
{
	public class Client
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long TelegramId { get; set; }

		public string? Name { get; set; }

		public DateTime RegisteredAt { get; set; } = DateTime.Now;
	}
}