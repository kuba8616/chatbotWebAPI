using ChatbotAI.Domain.Enums;

namespace ChatbotAI.Domain.Entities;
public class ChatResponse
{
    public int Id { get; set; }
    public int ChatMessageId { get; set; } 
    public ChatMessage ChatMessage { get; set; } = null!;
    public Rating Rating { get; set; }
}
