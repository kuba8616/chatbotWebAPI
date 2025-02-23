namespace ChatbotAI.Application.Interfaces.Services;
public interface IChatbotResponseGenerator
{
    Task<string> GenerateResponseAsync(string message, CancellationToken cancellationToken);
}
