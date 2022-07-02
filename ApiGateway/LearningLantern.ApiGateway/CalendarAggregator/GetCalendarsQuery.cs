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

    public GetCalendarsQueryHandler(CalendarService calendarService, ILearningLanternContext context)
    {
        _calendarService = calendarService;
        _context = context;
    }

    public async Task<Response<IEnumerable<CalendarEventDTO>>> Handle(
        GetCalendarsQuery request, CancellationToken cancellationToken)
    {
        var classroomIds = _context.ClassroomUsers.Where(x => x.UserId == request.User.Id)
            .Select(x => x.ClassroomId);
        var result = await _calendarService.GetAllCalendarsAsync(classroomIds);
        return result;
    }
}