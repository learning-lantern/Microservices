using LearningLantern.ApiGateway.Data;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.Common.Response;
using MediatR;

namespace LearningLantern.ApiGateway.CalendarAggregator;

public class GetCalendarsQuery : AuthorizedRequest<Response<IEnumerable<CalendarEventDTO>>>
{
}

public class GetCalendarsQueryHandler : IRequestHandler<GetCalendarsQuery, Response<IEnumerable<CalendarEventDTO>>>
{
    private readonly CalendarService _calendarService;
    private readonly ILearningLanternContext _context;
    private readonly TodoService _todoService;

    public GetCalendarsQueryHandler(
        CalendarService calendarService, TodoService todoService, ILearningLanternContext context)
    {
        _calendarService = calendarService;
        _todoService = todoService;
        _context = context;
    }

    public async Task<Response<IEnumerable<CalendarEventDTO>>> Handle(
        GetCalendarsQuery request, CancellationToken cancellationToken)
    {
        var classroomIds = _context.ClassroomUsers.Where(x => x.UserId == request.User.Id)
            .Select(x => x.ClassroomId);

        var calendarTask = _calendarService.GetAllCalendarsAsync(classroomIds);
        var todoTask = _todoService.GetAllTasksAsync(request.User.Id);
        await Task.WhenAll(calendarTask, todoTask);
        var calendarsResult = calendarTask.Result;
        var todoResult = todoTask.Result;


        var result = UnionData(calendarsResult.Data, todoResult.Data);
        var errors = UnionData(calendarsResult.Errors, todoResult.Errors);

        if (calendarsResult.Succeeded || todoResult.Succeeded) return ResponseFactory.Ok(result);

        return ResponseFactory.Fail<IEnumerable<CalendarEventDTO>>(errors);
    }

    private static IEnumerable<T> UnionData<T>(params IEnumerable<T>?[] arr)
    {
        IEnumerable<T> result = Array.Empty<T>();
        result = arr.Aggregate(result, (current, x) => current.Union(x ?? Array.Empty<T>()));
        return result;
    }
}