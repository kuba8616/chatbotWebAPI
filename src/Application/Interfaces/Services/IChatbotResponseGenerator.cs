namespace ChatbotAI.Application.Interfaces.Services;
public interface IChatbotResponseGenerator
{
    Task<string> GenerateResponseAsync(string userMessage, int responseId, CancellationToken cancellationToken);
    void CancelResponse(int responseId);
}
