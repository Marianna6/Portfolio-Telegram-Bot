using System;
using System.Collections.Generic;

namespace MyProjects.Tests;

public static class BotLogicTestData
{

	public static long NewId => BitConverter.ToInt64(Guid.NewGuid().ToByteArray());
	public static string NewName => $"User_{Guid.NewGuid().ToString().Substring(0, 5)}";

	public static IEnumerable<object[]> GetBotCommands =>
		new List<object[]>
		{
			new object[] { "services", "Choose your service:" },
			new object[] { "booking", "To simplify, I automatically book" },
			new object[] { "unknown_text", "Sorry, I didn't understand that." }
		};

	public static IEnumerable<object[]> BookingData =>
		new List<object[]>
		{
			new object[] { "/book_massage", "Massage" },
			new object[] { "/book_facial", "Facial" }
		};
}
