using ChatbotAI.Application.Common.Models;
using MediatR;

namespace ChatbotAI.Application.Chat.Commands.AddChatMessage;
public class AddChatMessageCommand : IRequest<Result<ChatResponseDto>>
{
    public string Content { get; set; } = string.Empty;
}
