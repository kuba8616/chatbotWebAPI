using ChatbotAI.Application.Chat.Commands.RateChatResponse;
using ChatbotAI.Application.Common;
using ChatbotAI.Application.Common.Constants;
using ChatbotAI.Domain.Entities;
using ChatbotAI.Domain.Enums;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatbotAI.Application.UnitTests.Chat.Commands.RateChatResponse
{
    [TestFixture]
    public class RateChatResponseCommandHandlerTests
    {
        private Mock<IApplicationDbContext> _dbContextMock;
        private RateChatResponseCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _dbContextMock = new Mock<IApplicationDbContext>();
            _handler = new RateChatResponseCommandHandler(_dbContextMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenResponseExists()
        {
            // Arrange
            var command = new RateChatResponseCommand { ResponseId = 1, Rating = Rating.Like };
            var cancellationToken = CancellationToken.None;
            var chatResponse = new ChatResponse { Id = command.ResponseId };

            _dbContextMock.Setup(db => db.ChatResponses.FindAsync(command.ResponseId))
                .ReturnsAsync(chatResponse);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            result.IsSuccess.Should().BeTrue();
            chatResponse.Rating.Should().Be(Rating.Like);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenResponseDoesNotExist()
        {
            // Arrange
            var command = new RateChatResponseCommand { ResponseId = 999, Rating = Rating.Like };
            var cancellationToken = CancellationToken.None;

            _dbContextMock.Setup(db => db.ChatResponses.FindAsync(command.ResponseId))
                .ReturnsAsync((ChatResponse?)null);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(ErrorMessages.ChatResponseNotFound);
        }
    }

}
