using ChatbotAI.Application.Interfaces.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChatbotAI.Application.UnitTests.Services;
[TestFixture]
public class ChatbotResponseGeneratorTests
{
    private Mock<IChatbotResponseGenerator> _chatbotResponseGeneratorMock;

    [SetUp]
    public void Setup()
    {
        _chatbotResponseGeneratorMock = new Mock<IChatbotResponseGenerator>();
    }

    [Test]
    public async Task GenerateResponseAsync_ShouldReturnExpectedResponse()
    {
        // Arrange
        var inputMessage = "Hello Message!";
        var expectedResponse = "Hello Response";
        var cancellationToken = CancellationToken.None;

        _chatbotResponseGeneratorMock.Setup(gen => gen.GenerateResponseAsync(inputMessage, 1, cancellationToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var response = await _chatbotResponseGeneratorMock.Object.GenerateResponseAsync(inputMessage, 1, cancellationToken);

        // Assert
        response.Should().Be(expectedResponse);
    }
}
