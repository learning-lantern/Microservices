using System.Net;
using LearningLantern.ApiGateway.Classroom.Repositories;
using LearningLantern.ApiGateway.Helpers;
using LearningLantern.Common;

namespace LearningLantern.ApiGateway.DelegatingHandlers;

public class ClassroomHandler : DelegatingHandler
{
    private readonly IClassroomRepository _classroomRepository;
    private readonly ICurrentUserService _currentUserService;


    public ClassroomHandler(ICurrentUserService currentUserService, IClassroomRepository classroomRepository)
    {
        _currentUserService = currentUserService;
        _classroomRepository = classroomRepository;
    }

    public ClassroomHandler(
        HttpMessageHandler innerHandler, ICurrentUserService currentUserService, IClassroomRepository classroomRepository) :
        base(innerHandler)
    {
        _currentUserService = currentUserService;
        _classroomRepository = classroomRepository;
    }

    async protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var classroomId = int.Parse(request.GetQueryString("classroomId") ?? "0");
        var userId = _currentUserService.UserId ?? "";
        var classroom = (await _classroomRepository.GetAsync(userId)).FirstOrDefault(dto => dto.Id == classroomId);

        if (classroom is not null) return await base.SendAsync(request, cancellationToken);

        var response = new HttpResponseMessage();
        response.StatusCode = HttpStatusCode.NotFound;
        //TODO: return response object
        return response;
    }
}