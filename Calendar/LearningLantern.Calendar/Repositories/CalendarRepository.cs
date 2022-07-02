using System.Linq.Expressions;
using AutoMapper;
using LearningLantern.Calendar.Data;
using LearningLantern.Calendar.Data.Models;
using LearningLantern.Calendar.Utility;
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

    public async Task<Response<EventModel>> AddAsync(AddEventDTO addEventProperties)
    {
        var tempEvent = _mapper.Map<EventModel>(addEventProperties);
        var eventModel = await _context.Events.AddAsync(tempEvent);

        var result = await _context.SaveChangesAsync();

        return result == 0 ? ResponseFactory.Fail<EventModel>() : ResponseFactory.Ok(eventModel.Entity);
    }

    public async Task<Response<EventModel>> GetEventByIdAsync(int eventId)
    {
        var eventModel = await GetEvents(eventModel => eventModel.Id == eventId).FirstOrDefaultAsync();

        return eventModel is null
            ? ResponseFactory.Fail<EventModel>(ErrorsList.EventNotFound(eventId))
            : ResponseFactory.Ok(eventModel);
    }

    public async Task<Response<EventModel>> UpdateAsync(int eventId, EventProperties eventProperties)
    {
        var eventModel = await GetEvents(eventModel => eventModel.Id == eventId).FirstOrDefaultAsync();

        if (eventModel is null) ResponseFactory.Fail(ErrorsList.EventNotFound(eventId));

        eventModel!.Update(eventProperties);
        _context.Events.Update(eventModel);

        var result = await _context.SaveChangesAsync() != 0;
        return result ? ResponseFactory.Ok(eventModel) : ResponseFactory.Fail<EventModel>();
    }

    public async Task<Response<int>> RemoveAsync(int eventId)
    {
        var task = await _context.Events.Where(eventModel => eventModel.Id == eventId).FirstOrDefaultAsync();

        if (task == null) return ResponseFactory.Ok(eventId);

        _context.Events.Remove(task);

        var result = await _context.SaveChangesAsync() != 0;

        return result ? ResponseFactory.Ok(eventId) : ResponseFactory.Fail<int>();
    }

    public async Task<Response<IEnumerable<EventModel>>> GetAsync(IEnumerable<int> classroomIds)
    {
        IEnumerable<EventModel> result =
            await GetEvents(model => classroomIds.Any(x => x == model.ClassroomId)).ToListAsync();
        return ResponseFactory.Ok(result);
    }

    private IQueryable<EventModel> GetEvents(Expression<Func<EventModel, bool>> filter) => _context.Events.Where(filter);
}