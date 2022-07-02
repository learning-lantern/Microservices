using LearningLantern.ApiGateway.Configurations;
using LearningLantern.Common.Response;

namespace LearningLantern.ApiGateway.CalendarAggregator;

public class TodoService
{
    private readonly HttpClient _httpClient;

    public TodoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = ConfigProvider.TodoListGetTaskEndPoint;
    }

    public async Task<Response<IEnumerable<CalendarEventDTO>>> GetAllTasksAsync(string userId)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<CalendarEventDTO>>(userId);
            if (result is not null)
                return ResponseFactory.Ok(result);
        }
        catch (Exception)
        {
            //ignored
        }

        return ResponseFactory.Fail<IEnumerable<CalendarEventDTO>>();
    }
}