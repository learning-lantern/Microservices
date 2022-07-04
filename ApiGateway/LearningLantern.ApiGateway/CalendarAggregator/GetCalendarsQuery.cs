using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Users.BuildingBlocks;
using LearningLantern.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningLantern.ApiGateway.CalendarAggregator;

public class GetCalendarsQuery : AuthorizedRequest<Response<IEnumerable<CalendarEventDTO>>>
{
}

public class GetCalendarsQueryHandler : IRequestHandler<GetCalendarsQuery, Response<IEnumerable<CalendarEventDTO>>>
{
    private readonly CalendarService _calendarService;
    private readonly TodoService _todoService;
    private readonly UserManager<UserModel> _userManager;

    public GetCalendarsQueryHandler(
        CalendarService calendarService, TodoService todoService, UserManager<UserModel> userManager)
    {
        _calendarService = calendarService;
        _todoService = todoService;
        _userManager = userManager;
    }

    public async Task<Response<IEnumerable<CalendarEventDTO>>> Handle(
        GetCalendarsQuery request, CancellationToken cancellationToken)
    {
        var classroomIds = _userManager.Users.Where(x => x.Id == request.User.Id)
            .SelectMany(x => x.Classrooms).Select(x => x.Id);

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