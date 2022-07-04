using LearningLantern.Calendar.Data.Models;
using LearningLantern.Common.Response;

namespace LearningLantern.Calendar.Repositories;

public interface ICalendarRepository
{
    public Task<Response<EventModel>> AddAsync(AddEventDTO addEventProperties);
    public Task<Response<IEnumerable<EventModel>>> GetAsync(IEnumerable<string> classroomIds);
    public Task<Response<EventModel>> GetEventByIdAsync(int eventId);
    public Task<Response<EventModel>> UpdateAsync(int eventId, EventProperties eventProperties);
    public Task<Response<int>> RemoveAsync(int eventId);
}