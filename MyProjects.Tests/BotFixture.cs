using TelegramBotApp.Data;
using TelegramBotApp.Logic;

namespace MyProjects.Tests
{
	public class BotFixture
	{
		public BotLogic Logic { get; }

		public BotFixture()
		{
			using var db = new ApplicationDbContext();
			db.Database.EnsureCreated();
			Logic = new BotLogic();
		}
	}
}