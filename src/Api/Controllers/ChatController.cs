using ChatbotAI.Application.Chat.Queries.GetChatHistory;
using ChatbotAI.Application.Common.Models;
using ChatbotAI.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class ChatController : BaseController
{
    public ChatController(IMediator mediator) : base(mediator) { }

    [HttpGet("history")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<List<ChatMessageDto>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<List<ChatMessageDto>>))]
    public async Task<IActionResult> GetChatHistory()
    {
        var result = await _mediator.Send(new GetChatHistoryQuery());

        return HandleResult(result);
    }
}
