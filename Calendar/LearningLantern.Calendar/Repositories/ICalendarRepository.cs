using LearningLantern.Common.Models.CalendarModels;
using LearningLantern.Common.Result;

namespace LearningLantern.Calendar.Repositories;

public interface ICalendarRepository
{
    public Task<Result<EventModel>> AddAsync(EventDTO eventDTO);
    public Task<Result<IEnumerable<EventModel>>> GetAsync(int classroomId);
    public Task<Result<EventModel>> GetEventByIdAsync(int eventId);
    public Task<Result> UpdateAsync(int eventId, UpdateEventDTO updateEventDTO);
    public Task<Result> RemoveAsync(int eventId);
}