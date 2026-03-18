using TelegramBotApp.Data;
using TelegramBotApp.Logic;

namespace MyProjects.Tests
{
	public class BotFixture : IDisposable
	{
		public BotLogic Logic { get; }
		public ApplicationDbContext Db { get; }

		public BotFixture()
		{
			Db = new ApplicationDbContext();
			Db.Database.EnsureCreated();
			Logic = new BotLogic();
		}

		public void Dispose()
		{
			Db.Dispose();
		}
	}
}