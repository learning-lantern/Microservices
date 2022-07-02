using System.Net;
using LearningLantern.ApiGateway.Classroom.Repositories;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Response;
using LearningLantern.Common.Services;
using Newtonsoft.Json;

namespace LearningLantern.ApiGateway.Ocelot.DelegatingHandlers;

internal class ClassroomIdType
{
    public int ClassroomId { get; set; }
}

public class ClassroomHandler : DelegatingHandler
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ClassroomHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public ClassroomHandler(HttpMessageHandler innerHandler, IServiceScopeFactory scopeFactory) :
        base(innerHandler)
    {
        _scopeFactory = scopeFactory;
    }

    async protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //setup
        using var scope = _scopeFactory.CreateScope();
        var currentUserService = scope.ServiceProvider.GetService<ICurrentUserService>()!;
        var classroomRepository = scope.ServiceProvider.GetRequiredService<IClassroomRepository>();
        var _logger = scope.ServiceProvider.GetRequiredService<ILogger<ClassroomHandler>>();
        //code

        var jsonRequest = await request.Content?.ReadAsStringAsync(cancellationToken)!;
        var obj = JsonConvert.DeserializeObject<ClassroomIdType>(jsonRequest);
        _logger.LogInformation(obj.ToJsonStringContent());
        var classroomId = obj!.ClassroomId;
        var userId = currentUserService.UserId ?? string.Empty;

        var classroom = (await classroomRepository.GetAsync(userId)).Data!.FirstOrDefault(dto => dto.Id == classroomId);

        if (classroom is not null) return await base.SendAsync(request, cancellationToken);

        var result = ResponseFactory.Fail(ErrorsList.ClassroomIdNotFound(classroomId));

        var response = new HttpResponseMessage(HttpStatusCode.NotFound);
        response.Content = new StringContent(result.Errors.ToJsonStringContent());

        return response;
    }
}