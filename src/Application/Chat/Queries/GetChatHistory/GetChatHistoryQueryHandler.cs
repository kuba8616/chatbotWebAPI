using ChatbotAI.Application.Common;
using ChatbotAI.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatbotAI.Application.Chat.Queries.GetChatHistory;
public class GetChatHistoryQueryHandler : IRequestHandler<GetChatHistoryQuery, Result<List<ChatMessageDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetChatHistoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ChatMessageDto>>> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.ChatMessages
            .Include(c => c.Response)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new ChatMessageDto
            {
                Id = c.Id,
                Message = c.Content,
                Response = c.Response != null ? c.Response.Content : "",
                Rating = c.Response != null ? c.Response.Rating : null
            })
            .ToListAsync(cancellationToken);

        return result.Any()
            ? Result<List<ChatMessageDto>>.Ok(result)
            : Result<List<ChatMessageDto>>.Fail("No chat history found.");
    }
}
