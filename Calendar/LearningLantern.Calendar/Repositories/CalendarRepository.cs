using System.Linq.Expressions;
using AutoMapper;
using LearningLantern.Calendar.Data;
using LearningLantern.Calendar.Data.Models;
using LearningLantern.Calendar.Exceptions;
using LearningLantern.Common.Response;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.Calendar.Repositories;

public class CalendarRepository : ICalendarRepository
{
    private readonly ICalendarContext _context;
    private readonly IMapper _mapper;

    public CalendarRepository(ICalendarContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Response<EventModel>> AddAsync(EventDTO eventDTO)
    {
        var tempEvent = _mapper.Map<EventModel>(eventDTO);
        var eventModel = await _context.Events.AddAsync(tempEvent);

        var result = await _context.SaveChangesAsync();

        if (result == 0) throw new CreateEventFailedException();

        return ResponseFactory.Ok(eventModel.Entity);
    }

    public async Task<Response<IEnumerable<EventModel>>> GetAsync(int classroomId)
    {
        IEnumerable<EventModel> result = await GetEvents(eventModel => eventModel.ClassroomId == classroomId).ToListAsync();
        return ResponseFactory.Ok(result);
    }

    public async Task<Response<EventModel>> GetEventByIdAsync(int eventId)
    {
        var eventModel = await GetEvents(eventModel => eventModel.Id == eventId).FirstOrDefaultAsync();

        if (eventModel is null) throw new EventNotFoundException();

        return ResponseFactory.Ok(eventModel);
    }

    public async Task<Response> UpdateAsync(int eventId, UpdateEventDTO updateEventDTO)
    {
        var eventModel = await GetEvents(eventModel => eventModel.Id == eventId).FirstOrDefaultAsync();

        if (eventModel is null) throw new EventNotFoundException();

        eventModel.Update(updateEventDTO);
        _context.Events.Update(eventModel);

        var result = await _context.SaveChangesAsync() != 0;
        return result ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }

    public async Task<Response> RemoveAsync(int eventId)
    {
        var task = await _context.Events.Where(eventModel => eventModel.Id == eventId).FirstOrDefaultAsync();

        if (task == null) return ResponseFactory.Ok();

        _context.Events.Remove(task);

        var result = await _context.SaveChangesAsync() != 0;

        return result ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }

    private IQueryable<EventModel> GetEvents(Expression<Func<EventModel, bool>> filter) => _context.Events.Where(filter);
}