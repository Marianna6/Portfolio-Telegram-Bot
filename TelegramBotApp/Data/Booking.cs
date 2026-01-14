using TelegramBotApp.Data;

public class Booking
{

	public int BookingId { get; set; }

	public DateTime Date { get; set; }

	public string? ServiceName { get; set; }

	public long ClientTelegramId { get; set; }

	public Client? Client { get; set; }
}