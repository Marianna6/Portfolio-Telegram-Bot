namespace TelegramBotApp.Logic
{
	public interface IBotLogic
	{
		string GetResponse(long chatId, string name, string messageText);
	}
}
