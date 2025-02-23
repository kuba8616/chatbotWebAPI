using ChatbotAI.Application.Chat.Commands.AddChatMessage;
using ChatbotAI.Application.Chat.Commands.CancelChatResponse;
using ChatbotAI.Application.Chat.Commands.RateChatResponse;
using ChatbotAI.Application.Chat.Queries.GetChatHistory;
using ChatbotAI.Application.Common.Models;
using ChatbotAI.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class ChatController : BaseController
{
    public ChatController(IMediator mediator) : base(mediator) { }

    [HttpGet("history")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<List<ChatResponseDto>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<List<ChatResponseDto>>))]
    public async Task<IActionResult> GetChatHistory()
    {
        var result = await _mediator.Send(new GetChatHistoryQuery());

        return HandleResult(result);
    }

    [HttpPost("messages")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Result<ChatResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<ChatResponseDto>))]
    public async Task<IActionResult> AddChatMessage([FromBody] AddChatMessageCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPut("responses/{responseId}/rate")]
    public async Task<IActionResult> RateChatResponse(int responseId, [FromBody] RateChatResponseCommand command)
    {
        if (responseId != command.ResponseId)
        {
            return BadRequest("Mismatched response ID.");
        }

        var result = await _mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("cancel")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<bool>))]
    public async Task<IActionResult> CancelResponse([FromBody] CancelChatResponseCommand command)
    {
        var result = await _mediator.Send(command);
        return HandleResult(result);
    }
}
