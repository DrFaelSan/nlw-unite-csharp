using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Attendees.GetAllByEventId;
using PassIn.Application.UseCases.Events.RegisterAttendee;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;

namespace PassIn.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AttendeesController : ControllerBase
{
    [HttpPost]
    [Route("{eventId}/register")]
    [ProducesResponseType(typeof(ResponseRegisteredJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status409Conflict)]
    public IActionResult Register([FromRoute] Guid eventId, [FromBody] RequestRegisterEventJson request)
    {

        RegisterAttendeeOnEventUseCase useCase = new();

        ResponseRegisteredJson response = useCase.Execute(eventId, request);

        return Created(string.Empty, response.Id);
    }

    [HttpGet]
    [Route("{eventId}")]
    [ProducesResponseType(typeof(ResponseAllAttendeesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public IActionResult GetAllAttendees([FromRoute] Guid eventId)
    {
        GetAllAttendeesByEventIdUseCase? useCase = new();

        ResponseAllAttendeesJson response = useCase.Execute(eventId);

        return Ok(response);
    }

}
