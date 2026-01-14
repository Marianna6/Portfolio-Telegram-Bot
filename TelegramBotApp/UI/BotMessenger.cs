using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotApp.Logic;

namespace TelegramBotApp.UI
{
	public class BotMessenger
	{
		private readonly TelegramBotClient _botClient;
		private readonly BotLogic _logic;
		private CancellationTokenSource _cts;

		public BotMessenger(string token)
		{
			_botClient = new TelegramBotClient(token);
			_logic = new BotLogic();
			_cts = new CancellationTokenSource();
		}

		public async Task Start()
		{
			var receiverOptions = new ReceiverOptions
			{
				AllowedUpdates = Array.Empty<UpdateType>()
			};

			_botClient.StartReceiving(
				HandleUpdateAsync,
				HandleErrorAsync,
				receiverOptions,
				cancellationToken: _cts.Token
			);

			var me = await _botClient.GetMe();
			Console.WriteLine($"Bot {me.Username} is running... Press Enter to stop.");
		}

		public void Stop()
		{
			_cts.Cancel();
		}

		private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			if (update.Message == null || update.Message.Text == null)
				return;

			var chatId = update.Message.Chat.Id;
			var userText = update.Message.Text;
			string name = update.Message.Chat.FirstName ?? "Stranger";

			Console.WriteLine($"Received: '{userText}' from {name} ({chatId})");

			string response = _logic.GetResponse(chatId, name, userText);

			await botClient.SendMessage(
				chatId: chatId,
				text: response,
				cancellationToken: cancellationToken
			);
		}

		private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
		{
			Console.WriteLine($"Error: {exception.Message}");
			return Task.CompletedTask;
		}
	}
}