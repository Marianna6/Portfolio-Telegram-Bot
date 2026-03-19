using Telegram.Bot.Types;

namespace MyProjects.Tests.TestData
{
	public static class BotMessengerTestData
	{
		public static IEnumerable<object[]> GetInvalidUpdates => new List<object[]>
		{
			new object[] { new Update { Message = null } },
			new object[] { new Update { Message = new Message { Text = null } } },
			new object[] { new Update { Message = new Message { Text = "" } } }
		};

		public static Update ValidUpdate => new Update
		{
			Message = new Message
			{
				Text = "Hello",
				Chat = new Chat { Id = 345974509, FirstName = "М" }
			}
		};

		public static Exception ApiError => new Exception("Test API error");
	}
}
