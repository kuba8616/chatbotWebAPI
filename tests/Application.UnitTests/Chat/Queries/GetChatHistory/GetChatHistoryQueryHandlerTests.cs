using ChatbotAI.Application.Chat.Queries.GetChatHistory;
using ChatbotAI.Application.Common;
using ChatbotAI.Domain.Entities;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;

namespace ChatbotAI.Application.UnitTests.Chat.Queries.GetChatHistory;
public class GetChatHistoryQueryHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;
    private GetChatHistoryQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _handler = new GetChatHistoryQueryHandler(_dbContextMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnChatHistory_WhenMessagesExist()
    {
        // Arrange
        var query = new GetChatHistoryQuery();
        var cancellationToken = CancellationToken.None;
        var messages = new List<ChatMessage> { new ChatMessage { Id = 1, Content = "Hello" } };

        _dbContextMock.Setup(db => db.ChatMessages).ReturnsDbSet(messages);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(1);
    }
}

