using System;
using System.Collections.Generic;

namespace MyProjects.Tests
{
	public static class BotLogicTestData
	{

		public static long NewId => BitConverter.ToInt64(Guid.NewGuid().ToByteArray());
		public static string NewName => $"User_{Guid.NewGuid().ToString().Substring(0, 5)}";

		public static IEnumerable<object[]> GetBotCommands =>
			new List<object[]>
			{
				new object[] { NewId, NewName, "services", "Choose your service:\n" +
				   "1. Massage (€40) -> type: /book_massage\n" +
				   "2. Facial (€50) -> type: /book_facial" },
				new object[] { NewId, NewName, "booking", "To simplify, I automatically book for Tomorrow.\n Just type: /book_massage or /book_facial" },
				new object[] { NewId, NewName, "unknown_text", "Sorry, I didn't understand that. Type /start to begin." }
			};

		public static IEnumerable<object[]> RegistrationData => new List<object[]>
	    {
		  new object[] { NewId, NewName }
        };

		public static IEnumerable<object[]> BookingData => new List<object[]>
	    {
		  new object[] { NewId, NewName, "/book_massage", "Massage" },
		  new object[] { NewId, NewName, "/book_facial", "Facial" }
	    };
	}
}
