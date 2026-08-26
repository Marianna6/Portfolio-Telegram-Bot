using Telegram.Bot;
using TelegramBotApp.Data;
using TelegramBotApp.Logic;
using TelegramBotApp.UI;

namespace TelegramBotApp.Tests.Tests
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

			DotNetEnv.Env.Load();

			var botToken = Environment.GetEnvironmentVariable("MY_TG_TOKEN");
			if (string.IsNullOrEmpty(botToken))
				throw new InvalidOperationException("Bot token is empty, check environment variables.");
			
			BotClient = new TelegramBotClient(botToken);
			Messenger = new BotMessenger(botToken, Logic);
		}

		public void Dispose()
		{
			Db.Dispose();
		}
	}
}