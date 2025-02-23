using ChatbotAI.Application.Common;
using ChatbotAI.Application.Common.Models;
using ChatbotAI.Application.Interfaces.Services;
using ChatbotAI.Domain.Entities;
using ChatbotAI.Domain.Enums;
using MediatR;

namespace ChatbotAI.Application.Chat.Commands.AddChatMessage
{
    public class AddChatMessageCommandHandler : IRequestHandler<AddChatMessageCommand, Result<ChatResponseDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IChatbotResponseGenerator _chatbotResponseGenerator;
        private readonly TimeProvider _timeProvider;

        public AddChatMessageCommandHandler(IApplicationDbContext context, IChatbotResponseGenerator chatbotResponseGenerator, TimeProvider timeProvider)
        {
            _context = context;
            _chatbotResponseGenerator = chatbotResponseGenerator;
            _timeProvider = timeProvider;
        }

        public async Task<Result<ChatResponseDto>> Handle(AddChatMessageCommand request, CancellationToken cancellationToken)
        {
            var message = new ChatMessage
            {
                Content = request.Content,
                CreatedAt = _timeProvider.GetUtcNow().UtcDateTime
            };

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync(cancellationToken);

            string chatbotResponse;
            int responseId = request.ResponseId;

            try
            {
                chatbotResponse = await _chatbotResponseGenerator.GenerateResponseAsync(request.Content, responseId, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                chatbotResponse = "Generowanie odpowiedzi przerwane...";
            }

            var chatResponse = new ChatResponse
            {
                ChatMessageId = message.Id,
                Content = chatbotResponse,
                ChatMessage = message,
                Rating = Rating.None,
                IsCancelled = cancellationToken.IsCancellationRequested
            };

            _context.ChatResponses.Add(chatResponse);
            await _context.SaveChangesAsync(cancellationToken);

            chatResponse.Id = responseId;

            return Result<ChatResponseDto>.Ok(new ChatResponseDto
            {
                Id = chatResponse.Id,
                Message = message.Content,
                Response = chatResponse.Content,
                Rating = chatResponse.Rating,
                IsCancelled = chatResponse.IsCancelled
            });
        }
    }
}
