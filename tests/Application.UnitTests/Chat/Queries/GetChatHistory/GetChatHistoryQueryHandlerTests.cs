using ChatbotAI.Application.Chat.Queries.GetChatHistory;
using ChatbotAI.Application.Common;
using ChatbotAI.Domain.Entities;
using ChatbotAI.Domain.Enums;
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
        var chatMessages = new List<ChatMessage>
            {
                new ChatMessage { Id = 1, Content = "Pierwsza wiadomość", CreatedAt = DateTime.UtcNow.AddHours(-2) },
                new ChatMessage { Id = 2, Content = "Druga wiadomość", CreatedAt = DateTime.UtcNow.AddHours(-1) }
            };

        var chatResponses = new List<ChatResponse>
            {
                new ChatResponse { Id = 1, ChatMessageId = 1, Content = "Odpowiedź 1", Rating = Rating.None, IsCancelled = false, ChatMessage = chatMessages[0] },
                new ChatResponse { Id = 2, ChatMessageId = 2, Content = "Odpowiedź 2", Rating = Rating.Like, IsCancelled = null, ChatMessage = chatMessages[1] }
            };

        _dbContextMock.Setup(db => db.ChatResponses).ReturnsDbSet(chatResponses);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(2);
    }

    [Test]
    public async Task Handle_ReturnsEmptyList_WhenNoChatResponsesExist()
    {
        // Arrange
        var query = new GetChatHistoryQuery();
        var cancellationToken = CancellationToken.None;

        var chatResponses = new List<ChatResponse> { };

        _dbContextMock.Setup(db => db.ChatResponses).ReturnsDbSet(chatResponses);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(0);
    }
}

