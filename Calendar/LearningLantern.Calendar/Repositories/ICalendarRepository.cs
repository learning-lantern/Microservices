using LearningLantern.Common.Models.CalendarModels;
using LearningLantern.Common.Response;

namespace LearningLantern.Calendar.Repositories;

public interface ICalendarRepository
{
    public Task<Response<EventModel>> AddAsync(EventDTO eventDTO);
    public Task<Response<IEnumerable<EventModel>>> GetAsync(int classroomId);
    public Task<Response<EventModel>> GetEventByIdAsync(int eventId);
    public Task<Response> UpdateAsync(int eventId, UpdateEventDTO updateEventDTO);
    public Task<Response> RemoveAsync(int eventId);
}