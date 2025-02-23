using ChatbotAI.Application.Chat.Commands.AddChatMessage;
using ChatbotAI.Application.Common;
using ChatbotAI.Application.Common.Constants;
using ChatbotAI.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatbotAI.Application.Chat.Queries.GetChatHistory;
public class GetChatHistoryQueryHandler : IRequestHandler<GetChatHistoryQuery, Result<List<ChatResponseDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetChatHistoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ChatResponseDto>>> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.ChatResponses
            .Include(x => x.ChatMessage)
            .OrderBy(r => r.Id)
            .Select(r => new ChatResponseDto
            {
                Id = r.Id,
                Message = r.ChatMessage.Content,
                Response = r.Content,
                Rating = r.Rating,
                IsCancelled = r.IsCancelled
            })
            .ToListAsync(cancellationToken);

        return Result<List<ChatResponseDto>>.Ok(result);
            
    }
}
