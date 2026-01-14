# Telegram Bot: Database & Logic Testing (xUnit)

![Bot Demo](demo.mp4)

This project demonstrates the implementation and testing of a Telegram Bot that manages service bookings. It highlights skills in **Relational Databases (SQLite)**, **Entity Framework Core**, and **Integration Testing**.
The primary goal was to implement a "Clean Architecture" where the business logic is fully decoupled from the Telegram Messenger API, making it testable and independent.

## Architecture & Refactoring

The application is split into three distinct layers to ensure Separation of Concerns:

* **Data Layer** (`TelegramBotApp.Data`): Handles the SQLite database connection using **Entity Framework Core**. Contains the `Client` and `Booking` data models.
* **Logic Layer** (`TelegramBotApp.Logic`): The "Brain" of the bot (`BotLogic`). It handles routing via the main `GetResponse` method, performs validation (e.g., checking registration status), and executes database commands.
* **UI Layer** (`TelegramBotApp.UI`): Manages the interaction with the Telegram API. It processes incoming updates and passes user input to the Logic layer.
* **Tests** (`MyProjects.Tests`): A comprehensive xUnit test suite that verifies the logic and database integration.

## Test Strategy and Coverage

 This suite performs Integration Testing using a real SQLite database environment. This ensures that INSERT and DELETE operations actually work as expected.
 The test suite covers the full lifecycle of a user interaction using the "Arrange, Act, Assert" pattern.

* **Data-Driven Command Testing:** Used [Theory] and [InlineData] to verify that the central router (GetResponse) correctly handles various text commands like "services", "booking", or gibberish.
* **Database State Verification:** Verified that logic changes the database state correctly:
    * Registration: Assert.NotNull(savedUser) confirms a new client is actually saved.
    * Booking: Verified that a booking is linked to the correct TelegramId
	* Cancellation: Verified that /cancel physically removes the record (Assert.Null).
* **Encapsulation Testing:** Tested private methods (like CreateBooking) via the public entry point (GetResponse).
* **Negative & Security Testing:** Confirmed that a "Ghost User" (not in DB) receives an error when trying to book, and handling cancellation when no booking exists.

## Key Skills Demonstrated

* **C# & .NET 9:** Ability to write, run, and debug code to understand internal logic.
* **Entity Framework Core:** Setting up a test database context and defining data models (Clients, Bookings).
* **Relational Database (SQLite):** Experience with SQL-based data storage, validating tables and checking that data persists correctly.
* **LINQ:** Using basic queries (like .FirstOrDefault()) to find specific records in the database.
* **xUnit Framework:**
    * Writing clean tests using the "Arrange, Act, Assert" pattern.
    * Test Isolation: Ensuring a clean database state for every test run to avoid data conflicts.

## Technology Stack

* **Language:** C#
* **Framework:** .NET 9
* **Data:** SQLite + Entity Framework Core
* **Testing:** xUnit
* **Library:** Telegram.Bot