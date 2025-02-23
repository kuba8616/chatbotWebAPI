using ChatbotAI.Application.Chat.Commands.CancelChatResponse;
using ChatbotAI.Application.Common;
using ChatbotAI.Application.Common.Models;
using ChatbotAI.Application.Interfaces.Services;
using MediatR;

namespace ChatbotAI.Application.Chat.Commands
{
    public class CancelChatResponseCommandHandler : IRequestHandler<CancelChatResponseCommand, Result<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IChatbotResponseGenerator _chatbotResponseGenerator;

        public CancelChatResponseCommandHandler(
            IApplicationDbContext context,
            IChatbotResponseGenerator chatbotResponseGenerator)
        {
            _context = context;
            _chatbotResponseGenerator = chatbotResponseGenerator;
        }

        public async Task<Result<bool>> Handle(CancelChatResponseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _chatbotResponseGenerator.CancelResponse(request.ResponseId);

                var chatResponse = await _context.ChatResponses.FindAsync(request.ResponseId, cancellationToken);
                if (chatResponse != null)
                {
                    if (chatResponse.IsCancelled.HasValue && chatResponse.IsCancelled.Value)
                    {
                        return Result<bool>.Ok(false, "Odpowiedź została już wcześniej anulowana.");
                    }

                    chatResponse.IsCancelled = true;
                    chatResponse.Content = chatResponse.Content ?? "Generowanie odpowiedzi przerwane...";

                    _context.ChatResponses.Update(chatResponse);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    return Result<bool>.Ok(true, "Generowanie odpowiedzi zostało anulowane (task w trakcie).");
                }

                return Result<bool>.Ok(true, "Generowanie odpowiedzi zostało anulowane pomyślnie.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"Błąd podczas anulowania odpowiedzi: {ex.Message}" );
            }

        }
    }
}
