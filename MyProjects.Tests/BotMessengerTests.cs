using Shouldly;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotApp.Logic;
using TelegramBotApp.UI;

namespace MyProjects.Tests
{
	public class BotMessengerTests
	{
		private readonly TelegramBotClient _botClient;
		private readonly BotMessenger _messenger;
		private readonly BotLogic _logic;

		public BotMessengerTests()
		{
			var token = Environment.GetEnvironmentVariable("MY_TG_TOKEN");

			_botClient = new TelegramBotClient(token);
			_logic = new BotLogic();
			_messenger = new BotMessenger(token, _logic);
		}

		public static IEnumerable<object[]> GetInvalidUpdates =>new List<object[]>
	    {
		    new object[] { new Update { Message = null } },
		    new object[] { new Update { Message = new Message { Text = null } } },
		    new object[] { new Update { Message = new Message { Text = "" } } }
	    };

		[Fact]
		public async Task HandleUpdate_ValidText_SendsResponse()
		{

			var update = new Update
			{
				Message = new Message
				{
					Text = "Hello",
					Chat = new Chat { Id = 345974509, FirstName = "М" }
				}
			};

			await Should.NotThrowAsync(async () => await _messenger.HandleUpdateAsync(_botClient, update, CancellationToken.None));
		}

		[Theory]
		[MemberData(nameof(GetInvalidUpdates))]
		public async Task HandleUpdate_NullText_ShouldNotFail(Update testUpdate)
		{

			await Should.NotThrowAsync(async () => await _messenger.HandleUpdateAsync(_botClient, testUpdate, CancellationToken.None));
		}

		[Fact]
		public async Task HandleError_ShouldLogMessageAndNotThrow()
		{

			var exception = new Exception("Test API error");

			await Should.NotThrowAsync(async () => await _messenger.HandleErrorAsync(_botClient, exception, CancellationToken.None));
		}
	}
}
