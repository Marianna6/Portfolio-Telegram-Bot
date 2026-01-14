using Microsoft.EntityFrameworkCore;

namespace TelegramBotApp.Data
{

	public class ApplicationDbContext : DbContext
	{

		public DbSet<Client> Clients { get; set; }

		public DbSet<Booking> Bookings { get; set; }

		public ApplicationDbContext()
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=TelegramBot.db");
		}
	}
}