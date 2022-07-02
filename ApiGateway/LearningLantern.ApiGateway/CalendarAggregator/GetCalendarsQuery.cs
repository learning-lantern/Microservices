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
        CalendarService calendarService, ILearningLanternContext context, TodoService todoService)
    {
        _calendarService = calendarService;
        _context = context;
        _todoService = todoService;
    }

    public async Task<Response<IEnumerable<CalendarEventDTO>>> Handle(
        GetCalendarsQuery request, CancellationToken cancellationToken)
    {
        var classroomIds = _context.ClassroomUsers.Where(x => x.UserId == request.User.Id)
            .Select(x => x.ClassroomId);
        var calendarsResult = await _calendarService.GetAllCalendarsAsync(classroomIds);
        var todoResult = await _todoService.GetAllTasksAsync(request.User.Id);
        IEnumerable<CalendarEventDTO> result;
        IEnumerable<Error> errors;
        //result.Union(calendarsResult)
        return calendarsResult;
    }
}