using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatbotAI.Application.Chat.Commands.AddChatMessage;
using ChatbotAI.Application.Common;
using ChatbotAI.Application.Interfaces.Services;
using ChatbotAI.Domain.Entities;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatbotAI.Application.UnitTests.Chat.Commands.AddChatMessage;

[TestFixture]
public class AddChatMessageCommandHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;
    private Mock<IChatbotResponseGenerator> _chatbotResponseGeneratorMock;
    private Mock<TimeProvider> _mockTimeProvider;
    private AddChatMessageCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _chatbotResponseGeneratorMock = new Mock<IChatbotResponseGenerator>();
        _mockTimeProvider = new Mock<TimeProvider>();
        _mockTimeProvider.Setup(tp => tp.GetUtcNow()).Returns(() => new DateTimeOffset(2025, 2, 23, 12, 0, 0, TimeSpan.Zero));
        _handler = new AddChatMessageCommandHandler(_dbContextMock.Object, _chatbotResponseGeneratorMock.Object, _mockTimeProvider.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WhenMessageIsAdded()
    {
        // Arrange
        var command = new AddChatMessageCommand { Content = "Hello!", ResponseId = 1 };
        var cancellationToken = CancellationToken.None;
        var chatbotResponse = "Hello, how can I help you?";

        _chatbotResponseGeneratorMock.Setup(gen => gen.GenerateResponseAsync(command.Content, 1, cancellationToken))
            .ReturnsAsync(chatbotResponse);

        _dbContextMock.Setup(db => db.ChatMessages.Add(It.IsAny<ChatMessage>()));
        _dbContextMock.Setup(db => db.ChatResponses.Add(It.IsAny<ChatResponse>()));
        _dbContextMock.Setup(db => db.SaveChangesAsync(cancellationToken))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value?.Response.Should().Be(chatbotResponse);
    }
}
