using ChatbotAI.Application.Common;
using ChatbotAI.Application.Common.Constants;
using ChatbotAI.Application.Common.Models;
using MediatR;

namespace ChatbotAI.Application.Chat.Commands.RateChatResponse;
public class RateChatResponseCommandHandler : IRequestHandler<RateChatResponseCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public RateChatResponseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(RateChatResponseCommand request, CancellationToken cancellationToken)
    {
        var response = await _context.ChatResponses.FindAsync(request.ResponseId);

        if (response == null)
            return Result.Fail(ErrorMessages.ChatResponseNotFound);

        response.Rating = request.Rating;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
