using LearningLantern.ApiGateway.Configurations;
using LearningLantern.Common.Response;

namespace LearningLantern.ApiGateway.CalendarAggregator;

public class CalendarService
{
    private readonly HttpClient _httpClient;

    public CalendarService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = ConfigProvider.CalendarGetEventsEndPoint;
    }

    public async Task<Response<IEnumerable<CalendarEventDTO>>> GetAllCalendarsAsync(IEnumerable<int> classroomIds)
    {
        var queryString = "?" + string.Join("&", classroomIds.Select(x => $"ClassroomId={x}"));

        try
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<CalendarEventDTO>>(queryString);
            if (result is not null)
                return ResponseFactory.Ok(result);
        }
        catch (Exception)
        {
            // ignored
        }

        return ResponseFactory.Fail<IEnumerable<CalendarEventDTO>>();
    }
}