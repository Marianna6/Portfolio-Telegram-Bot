using TelegramBotApp.Data;
using TelegramBotApp.Logic;

namespace MyProjects.Tests
{
	public class BotLogicTests
	{
		public BotLogicTests()
		{
			using (var db = new ApplicationDbContext())
			{
				db.Database.EnsureCreated();
			}
		}

		[Theory]
		[InlineData("services", "Choose your service:\n1. Massage (€40) -> type: /book_massage\n2. Facial (€50) -> type: /book_facial")]
		[InlineData("booking", "To simplify, I automatically book for Tomorrow.\n Just type: /book_massage or /book_facial")]
		[InlineData("afjrikfjr", "Sorry, I didn't understand that. Type /start to begin.")]
		public void GetResponse_InputCommands_ReturnsCorrectMessage(string userMessage, string expectedResponse)
		{

			var logic = new BotLogic();
			long userId = DateTime.Now.Ticks;
			string userName = "Tester";

			string actualResponse = logic.GetResponse(userId, userName, userMessage);

			Assert.Equal(expectedResponse, actualResponse);
		}

		[Fact]
		public void RegisterUser_NewUser_SavesToDatabase()
		{

			var logic = new BotLogic();
			long userId = DateTime.Now.Ticks;
			string userName = "Tester";

			string response = logic.GetResponse(userId, userName, "/start");

			Assert.Contains($"Hi {userName}", response);

			using (var db = new ApplicationDbContext())
			{
				var savedUser = db.Clients.FirstOrDefault(c => c.TelegramId == userId);

				Assert.NotNull(savedUser);

				Assert.Equal(userName, savedUser.Name);
			}
		}

		[Fact]
		public void RegisterUser_ExistingUser_ReturnsWelcomeBack()
		{

			var logic = new BotLogic();
			long userId = DateTime.Now.Ticks;
			string userName = "Tester";

			logic.GetResponse(userId, userName, "/start");
			string response = logic.GetResponse(userId, userName, "/start");

			Assert.Contains("Welcome back", response);
			Assert.Contains(userName, response);
		}

		[Fact]
		public void GetResponse_BookMassage_CreatesBookingInDb()
		{
			var logic = new BotLogic();
			long userId = DateTime.Now.Ticks;
			string userName = "MassageBooker";
			string serviceName = "Massage";

			logic.GetResponse(userId, userName, "/start");

			string response = logic.GetResponse(userId, userName, "/book_massage");

			Assert.Contains($"Awesome! I booked a {serviceName}", response);

			using (var db = new ApplicationDbContext())
			{
				var booking = db.Bookings.FirstOrDefault(b => b.ClientTelegramId == userId);
				Assert.NotNull(booking);
				Assert.Equal(serviceName, booking.ServiceName);
				Assert.Equal(userId, booking.ClientTelegramId);
			}
		}

		[Fact]
		public void GetResponse_BookFacial_CreatesBookingInDb()
		{

			var logic = new BotLogic();
			long userId = DateTime.Now.Ticks;
			string userName = "FacialBooker";
			string serviceName = "Facial";

			logic.GetResponse(userId, userName, "/start");

			string response = logic.GetResponse(userId, userName, "/book_facial");

			Assert.Contains($"Awesome! I booked a {serviceName}", response);

			using (var db = new ApplicationDbContext())
			{
				var booking = db.Bookings.FirstOrDefault(b => b.ClientTelegramId == userId);
				Assert.NotNull(booking);
				Assert.Equal(serviceName, booking.ServiceName);
				Assert.Equal(userId, booking.ClientTelegramId);
			}
		}

		[Fact]
		public void GetResponse_Cancel_RemovesBooking()
		{

			var logic = new BotLogic();
			long userId = DateTime.Now.Ticks;
			string userName = "Canceller";

			logic.GetResponse(userId, userName, "/start");
			logic.GetResponse(userId, userName, "/book_massage");

			string response = logic.GetResponse(userId, userName, "/cancel");

			Assert.Equal("Your booking has been cancelled.", response);

			using (var db = new ApplicationDbContext())
			{
				var booking = db.Bookings.FirstOrDefault(b => b.ClientTelegramId == userId);
				Assert.Null(booking);
			}
		}

		[Fact]
		public void GetResponse_CreateBooking_WithoutRegistration_ReturnsError()
		{

			var logic = new BotLogic();
			long userId = DateTime.Now.Ticks;

			string response = logic.GetResponse(userId, "GhostUser", "/book_massage");

			Assert.Equal("Error: You are not registered. Please type /start first.", response);

			using (var db = new ApplicationDbContext())
			{

				var booking = db.Bookings.FirstOrDefault(b => b.ClientTelegramId == userId);

				Assert.Null(booking);
			}
		}

		[Fact]
		public void GetResponse_Cancel_NoBooking_ReturnsError()
		{
			var logic = new BotLogic();
			long userId = DateTime.Now.Ticks;
			string userName = "EmptyUser";

			logic.GetResponse(userId, userName, "/start");

			string response = logic.GetResponse(userId, userName, "/cancel");

			Assert.Equal("You have no active bookings to cancel.", response);
		}
	}
}
