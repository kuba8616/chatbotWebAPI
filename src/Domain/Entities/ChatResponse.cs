using ChatbotAI.Domain.Enums;

namespace ChatbotAI.Domain.Entities;
public class ChatResponse
{
    public int Id { get; set; }
    public int ChatMessageId { get; set; }
    public required ChatMessage ChatMessage { get; set; }
    public string Content { get; set; } = string.Empty;
    public Rating Rating { get; set; }
}
