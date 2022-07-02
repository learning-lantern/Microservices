using LearningLantern.ApiGateway.CalendarAggregator;
using LearningLantern.Common;
using LearningLantern.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.ApiGateway.Controllers;

[Route("api/[controller]")]
[Authorize]
public class CalendarController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public CalendarController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    [ProducesResponseType(typeof(Response<IEnumerable<CalendarEventDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var response = await _mediator.Send(new GetCalendarsQuery());
        return ResponseToIActionResult(response);
    }
}