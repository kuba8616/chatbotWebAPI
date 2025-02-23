using ChatbotAI.Application.Common.Models;
using ChatbotAI.Domain.Enums;
using MediatR;

namespace ChatbotAI.Application.Chat.Commands.RateChatResponse;
public class RateChatResponseCommand : IRequest<Result>
{
    public int ResponseId { get; set; }
    public Rating Rating { get; set; }
}
