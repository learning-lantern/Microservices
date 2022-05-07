namespace LearningLantern.ApiGateway.DelegatingHandlers;

public class ClassroomHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage>
        SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
        base.SendAsync(request, cancellationToken);
}