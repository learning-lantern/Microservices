using LearningLantern.Calendar.Data.Models;
using LearningLantern.Calendar.Repositories;
using LearningLantern.Common;
using LearningLantern.Common.Response;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.Calendar.Controllers;

[Route("/api/v1/[controller]")]
[ApiController]
public class EventsController : ApiControllerBase
{
    private readonly ICalendarRepository _calendarRepository;

    public EventsController(ICalendarRepository calendarRepository)
    {
        _calendarRepository = calendarRepository;
    }

    [HttpPost]
    [ProducesResponseType(typeof(EventModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] AddEventDTO addEventDTO)
    {
        var response = await _calendarRepository.AddAsync(addEventDTO);
        return ResponseToIActionResult(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EventModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromQuery] IEnumerable<int> classroomId)
    {
        var response = await _calendarRepository.GetAsync(classroomId);
        return ResponseToIActionResult(response);
    }

    [HttpGet("{eventId:int}")]
    [ProducesResponseType(typeof(EventModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEvent([FromRoute] int eventId)
    {
        var response = await _calendarRepository.GetEventByIdAsync(eventId);
        return ResponseToIActionResult(response);
    }

    [HttpPut("{eventId:int}")]
    [ProducesResponseType(typeof(EventModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int eventId, [FromBody] EventProperties eventProperties)
    {
        var response = await _calendarRepository.UpdateAsync(eventId, eventProperties);
        return ResponseToIActionResult(response);
    }

    [HttpDelete("{eventId:int}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int eventId)
    {
        var response = await _calendarRepository.RemoveAsync(eventId);
        return ResponseToIActionResult(response);
    }
}