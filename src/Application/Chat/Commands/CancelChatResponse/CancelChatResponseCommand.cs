using ChatbotAI.Application.Common.Models;
using MediatR;

namespace ChatbotAI.Application.Chat.Commands.CancelChatResponse;
public class CancelChatResponseCommand : IRequest<Result<bool>>
{
    public int ResponseId { get; set; }
}
