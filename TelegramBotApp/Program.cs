using TelegramBotApp.Data;
using TelegramBotApp.Logic;
using TelegramBotApp.UI;

namespace TelegramBotApp
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("Initializing database...");

			using (var db = new ApplicationDbContext())
			{
				bool created = db.Database.EnsureCreated();

				if (created)
				{
					Console.WriteLine("Database created successfully.");
				}
				else
				{
					Console.WriteLine("Database already exists.");
				}
			}

			var logic = new BotLogic();
			var bot = new BotMessenger("Token", logic);

			await bot.Start();

			Console.WriteLine("Press Enter to exit...");
			Console.ReadLine();

			bot.Stop();
		}
	}
}