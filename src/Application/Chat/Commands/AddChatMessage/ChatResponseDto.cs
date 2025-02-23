using ChatbotAI.Domain.Enums;

namespace ChatbotAI.Application.Chat.Commands.AddChatMessage;
public class ChatResponseDto
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public Rating Rating { get; set; }
    public bool? IsCancelled { get; set; }
}
