using LearningLantern.Calendar.Exceptions;
using LearningLantern.Calendar.Repositories;
using LearningLantern.Calendar.Utility;
using LearningLantern.Common;
using LearningLantern.Common.Models.CalendarModels;
using LearningLantern.Common.Response;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.Calendar.Controllers;

[Route("/api/v1/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly ICalendarRepository _calendarRepository;

    public EventsController(ICalendarRepository calendarRepository)
    {
        _calendarRepository = calendarRepository;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Response<EventModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] EventDTO eventDTO)
    {
        try
        {
            var eventModel = await _calendarRepository.AddAsync(eventDTO);
            return Ok(eventModel);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(Response<IEnumerable<EventModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] int classroomId)
    {
        var response = await _calendarRepository.GetAsync(classroomId);
        return Ok(response.ToJsonStringContent());
    }

    [HttpGet("{eventId:int}")]
    [ProducesResponseType(typeof(Response<EventModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<EventModel>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEvent([FromRoute] int eventId)
    {
        try
        {
            var response = await _calendarRepository.GetEventByIdAsync(eventId);
            return Ok(response.ToJsonStringContent());
        }
        catch (EventNotFoundException)
        {
            var response = ResponseFactory.Fail(ErrorsList.EventNotFound(eventId));
            return NotFound(response.ToJsonStringContent());
        }
    }

    [HttpPut("{eventId:int}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int eventId, [FromBody] UpdateEventDTO updateEventDTO)
    {
        var response = await _calendarRepository.UpdateAsync(eventId, updateEventDTO);
        if (response.Succeeded) return Ok(response);
        return NotFound(response.ToJsonStringContent());
    }

    [HttpDelete("{eventId:int}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int eventId)
    {
        var response = await _calendarRepository.RemoveAsync(eventId);

        if (response.Succeeded) return Ok(response);

        return BadRequest(response);
    }
}