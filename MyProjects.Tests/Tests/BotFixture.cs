using Telegram.Bot;
using TelegramBotApp.Data;
using TelegramBotApp.Logic;
using TelegramBotApp.UI;

namespace MyProjects.Tests.Tests
{
	public class BotFixture : IDisposable
	{
		public BotLogic Logic { get; }
		public ApplicationDbContext Db { get; }
		public TelegramBotClient BotClient { get; }
		public BotMessenger Messenger { get; }

		public BotFixture()
		{
			Db = new ApplicationDbContext();
			Db.Database.EnsureCreated();
			Logic = new BotLogic();

			var token = Environment.GetEnvironmentVariable("MY_TG_TOKEN");
			BotClient = new TelegramBotClient(token);
			Messenger = new BotMessenger(token, Logic);
		}

		public void Dispose()
		{
			Db.Dispose();
		}
	}
}