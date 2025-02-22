using ChatbotAI.Domain.Enums;

namespace ChatbotAI.Application.Chat.Queries.GetChatHistory;
public class ChatMessageDto
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public Rating? Rating { get; set; } = null;
}
