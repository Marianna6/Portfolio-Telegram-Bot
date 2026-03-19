using TelegramBotApp.Data;
using TelegramBotApp.Logic;
using Shouldly;
using MyProjects.Tests.TestData;

namespace MyProjects.Tests.Tests.BotLogicTests
{

	public class BotLogicTests : IClassFixture<BotFixture>
	{
		private readonly BotLogic _logic;
		private readonly ApplicationDbContext _db;

		public BotLogicTests(BotFixture fixture)
		{
			_logic = fixture.Logic;
			_db = fixture.Db;
		}

		[Theory]
		[MemberData(nameof(BotLogicTestData.GetBotCommands), MemberType = typeof(BotLogicTestData))]
		public void GetResponse_InputCommands_ReturnsCorrectMessage(long id, string name, string userMessage, string expectedResponse)
		{
			string actualResponse = _logic.GetResponse(id, name, userMessage);
			actualResponse.ShouldBe(expectedResponse);
		}

		[Theory]
		[MemberData(nameof(BotLogicTestData.RegistrationData), MemberType = typeof(BotLogicTestData))]
		public void RegisterUser_NewUser_SavesToDatabase(long id, string name)
		{

			string response = _logic.GetResponse(id, name, "/start");

			response.ShouldContain($"Hi {name}");

			var savedUser = _db.Clients.FirstOrDefault(c => c.TelegramId == id);

			savedUser.ShouldNotBeNull();
            savedUser.Name.ShouldBe(name);

		}

		[Theory]
		[MemberData(nameof(BotLogicTestData.RegistrationData), MemberType = typeof(BotLogicTestData))]
		public void RegisterUser_ExistingUser_ReturnsWelcomeBack(long id, string name)
		{

			_logic.GetResponse(id, name, "/start");
			string response = _logic.GetResponse(id, name, "/start");

			response.ShouldContain("Welcome back");
			response.ShouldContain(name);
		}

		[Theory]
		[MemberData(nameof(BotLogicTestData.BookingData), MemberType = typeof(BotLogicTestData))]
		public void BookService_CreatesBookingInDb(long id, string name, string serviceMessage, string expectedService)
		{

			_logic.GetResponse(id, name, "/start");

			string response = _logic.GetResponse(id, name, serviceMessage);

			response.ShouldContain($"Awesome! I booked a {expectedService}");

			var booking = _db.Bookings.FirstOrDefault(b => b.ClientTelegramId == id);

			booking.ShouldNotBeNull();
			booking.ServiceName.ShouldBe(expectedService);
		}

		[Theory]
		[MemberData(nameof(BotLogicTestData.RegistrationData), MemberType = typeof(BotLogicTestData))]
		public void GetResponse_Cancel_RemovesBooking(long id, string name)
		{

			_logic.GetResponse(id, name, "/start");
			_logic.GetResponse(id, name, "/book_massage");

			string response = _logic.GetResponse(id, name, "/cancel");

			response.ShouldBe("Your booking has been cancelled.");

			var booking = _db.Bookings.FirstOrDefault(b => b.ClientTelegramId == id);
			booking.ShouldBeNull();
		}

		[Theory]
		[MemberData(nameof(BotLogicTestData.RegistrationData), MemberType = typeof(BotLogicTestData))]
		public void GetResponse_CreateBooking_WithoutRegistration_ReturnsError(long id, string name)
		{

			string response = _logic.GetResponse(id, name, "/book_massage");

			response.ShouldBe("Error: You are not registered. Please type /start first.");

			var booking = _db.Bookings.FirstOrDefault(b => b.ClientTelegramId == id);

			booking.ShouldBeNull();
		}

		[Theory]
		[MemberData(nameof(BotLogicTestData.RegistrationData), MemberType = typeof(BotLogicTestData))]
		public void GetResponse_Cancel_NoBooking_ReturnsError(long id, string name)
		{

			_logic.GetResponse(id, name, "/start");

			string response = _logic.GetResponse(id, name, "/cancel");

			response.ShouldBe("You have no active bookings to cancel.");
		}
	}
}
