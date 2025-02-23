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
    private AddChatMessageCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _chatbotResponseGeneratorMock = new Mock<IChatbotResponseGenerator>();
        _handler = new AddChatMessageCommandHandler(_dbContextMock.Object, _chatbotResponseGeneratorMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WhenMessageIsAdded()
    {
        // Arrange
        var command = new AddChatMessageCommand { Content = "Hello!" };
        var cancellationToken = CancellationToken.None;
        var chatbotResponse = "Hello, how can I help you?";

        _chatbotResponseGeneratorMock.Setup(gen => gen.GenerateResponseAsync(command.Content, cancellationToken))
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
