using Shouldly;
using MyProjects.Tests.TestData;
using Telegram.Bot.Types;

namespace MyProjects.Tests.Tests.BotMessengerTests
{
	public class BotMessengerTests : IClassFixture<BotFixture>
	{
		private readonly BotFixture _fixture;

		public BotMessengerTests(BotFixture fixture)
		{
			_fixture = fixture;
		}

		[Fact]
		public async Task HandleUpdate_ValidText_SendsResponse()
		{

			await Should.NotThrowAsync(async () => await _fixture.Messenger.HandleUpdateAsync(_fixture.BotClient, BotMessengerTestData.ValidUpdate, CancellationToken.None));
		}

		[Theory]
		[MemberData(nameof(BotMessengerTestData.GetInvalidUpdates), MemberType = typeof(BotMessengerTestData))]
		public async Task HandleUpdate_NullText_ShouldNotFail(Update testUpdate)
		{

			await Should.NotThrowAsync(async () => await _fixture.Messenger.HandleUpdateAsync(_fixture.BotClient, testUpdate, CancellationToken.None));
		}

		[Fact]
		public async Task HandleError_ShouldLogMessageAndNotThrow()
		{

			await Should.NotThrowAsync(async () => await _fixture.Messenger.HandleErrorAsync(_fixture.BotClient, BotMessengerTestData.ApiError, CancellationToken.None));
		}
	}
}
