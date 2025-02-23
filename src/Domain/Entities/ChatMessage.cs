using ChatbotAI.Domain.Common;

namespace ChatbotAI.Domain.Entities
{
    public class ChatMessage : ICreatedAt
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsUserMessage { get; set; }
        public ChatResponse? Response { get; set; }
    }
}
