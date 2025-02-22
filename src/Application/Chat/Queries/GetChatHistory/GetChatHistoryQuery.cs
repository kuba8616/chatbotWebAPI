using ChatbotAI.Application.Common.Models;
using MediatR;

namespace ChatbotAI.Application.Chat.Queries.GetChatHistory;

public record GetChatHistoryQuery() : IRequest<Result<List<ChatMessageDto>>>;
