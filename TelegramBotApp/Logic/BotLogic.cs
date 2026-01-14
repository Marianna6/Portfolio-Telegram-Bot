using System;
using System.Linq;
using TelegramBotApp.Data;

namespace TelegramBotApp.Logic
{
	/// <summary>
	/// Handles user commands, database interactions, and response generation.
	/// Contains the core business logic of the bot.
	/// </summary>
	public class BotLogic
	{
		/// <summary>
		/// Main entry point.
		/// Processes the incoming message and returns the appropriate text response.
		/// </summary>
		/// <param name="chatId">Unique Telegram Chat ID.</param>
		/// <param name="name">User's First Name from Telegram.</param>
		/// <param name="messageText">The command/text sent by the user.</param>
		/// <returns>The text response to send back.</returns>
		public string GetResponse(long chatId, string name, string messageText)
		{
			return messageText.ToLower() switch
			{
				"/start" => RegisterUser(chatId, name),
				"services" => GetServicesMessage(),
				"booking" => GetBookingMessage(),
				"/book_massage" => CreateBooking(chatId, "Massage"),
				"/book_facial" => CreateBooking(chatId, "Facial"),
				"/cancel" => CancelBooking(chatId),
				_ => GetUnknownCommandMessage()
			};
		}

		/// <summary>
		/// Registers a new user or greets an existing one.
		/// </summary>
		private string RegisterUser(long Id, string name)
		{
			using (var db = new ApplicationDbContext())
			{

				var existingUser = db.Clients.FirstOrDefault(c => c.TelegramId == Id);

				if (existingUser == null)
				{

					var newClient = new Client
					{
						TelegramId = Id,
						Name = name,
						RegisteredAt = DateTime.Now
					};

					db.Clients.Add(newClient);
					db.SaveChanges();

					return $"Hi {name}! You have successfully registered.\n" +
						   "Type 'services' to see what we offer.";
				}
				else
				{
					return $"Welcome back, {name}! \n" +
						   "Type 'services' or 'booking'.";
				}
			}
		}

		/// <summary>
		/// Returns the list of available services with prices.
		/// Note: Currently limited to 2 services for demonstration purposes.
		/// </summary>
		public string GetServicesMessage()
		{
			return "Choose your service:\n" +
				   "1. Massage (€40) -> type: /book_massage\n" +
				   "2. Facial (€50) -> type: /book_facial";
		}

		/// <summary>
		/// Returns booking instructions.
		/// Note: Uses simplified booking logic for this MVP version.
		/// </summary>
		public string GetBookingMessage()
		{
			return "To simplify, I automatically book for Tomorrow.\n Just type: /book_massage or /book_facial";
		}

		public string GetUnknownCommandMessage()
		{
			return "Sorry, I didn't understand that. Type /start to begin.";
		}

		/// <summary>
		/// Creates a new booking for the user.
		/// Note: The date is automatically set to Tomorrow booking for simplicity.
		/// </summary>
		/// <param name="telegramId">User ID to link the booking.</param>
		/// <param name="serviceName">Name of the service (Massage/Facial).</param>
		/// <returns>Success message or registration error.</returns>
		private string CreateBooking(long telegramId, string serviceName)
		{
			using (var db = new ApplicationDbContext())
			{
				var client = db.Clients.FirstOrDefault(c => c.TelegramId == telegramId);
				if (client == null)
				{
					return "Error: You are not registered. Please type /start first.";
				}

				var newBooking = new Booking
				{
					ClientTelegramId = telegramId,
					ServiceName = serviceName,
					Date = DateTime.Now.AddDays(1)
				};

				db.Bookings.Add(newBooking);
				db.SaveChanges();

				return $"Awesome! I booked a {serviceName} for you for tomorrow!";
			}
		}

		/// <summary>
		/// Cancels the user's active booking if it exists.
		/// </summary>
		private string CancelBooking(long telegramId)
		{
			using (var db = new ApplicationDbContext())
			{

				var booking = db.Bookings.FirstOrDefault(b => b.ClientTelegramId == telegramId);

				if (booking == null)
				{
					return "You have no active bookings to cancel.";
				}

				db.Bookings.Remove(booking);
				db.SaveChanges();

				return "Your booking has been cancelled.";
			}
		}
	}
}
