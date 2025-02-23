using ChatbotAI.Application.Common;
using ChatbotAI.Application.Common.Models;
using ChatbotAI.Application.Interfaces.Services;
using ChatbotAI.Domain.Entities;
using ChatbotAI.Domain.Enums;
using MediatR;

namespace ChatbotAI.Application.Chat.Commands.AddChatMessage;
public class AddChatMessageCommandHandler : IRequestHandler<AddChatMessageCommand, Result<ChatResponseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IChatbotResponseGenerator _chatbotResponseGenerator;

    public AddChatMessageCommandHandler(IApplicationDbContext context, IChatbotResponseGenerator chatbotResponseGenerator)
    {
        _context = context;
        _chatbotResponseGenerator = chatbotResponseGenerator;
    }

    public async Task<Result<ChatResponseDto>> Handle(AddChatMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new ChatMessage
        {
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        _context.ChatMessages.Add(message);
        await _context.SaveChangesAsync(cancellationToken);

        var chatbotResponse = await _chatbotResponseGenerator.GenerateResponseAsync(request.Content, cancellationToken);

        var chatResponse = new ChatResponse
        {
            ChatMessageId = message.Id,
            Content = chatbotResponse,
            ChatMessage = message,
            Rating = Rating.None
        };

        _context.ChatResponses.Add(chatResponse);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<ChatResponseDto>.Ok(new ChatResponseDto
        {
            Id = chatResponse.Id,
            Message = message.Content,
            Response = chatResponse.Content,
            Rating = chatResponse.Rating
        });
    }
}
