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
		private readonly IBotLogic _logic;
		private CancellationTokenSource _cts;

		public BotMessenger(string token, IBotLogic logic)
		{
			_botClient = new TelegramBotClient(token);
			_logic = logic;
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

		public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			if (update.Message?.Text == null || string.IsNullOrWhiteSpace(update.Message.Text))
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

		public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
		{
			Console.WriteLine($"Error: {exception.Message}");
			return Task.CompletedTask;
		}
	}
}