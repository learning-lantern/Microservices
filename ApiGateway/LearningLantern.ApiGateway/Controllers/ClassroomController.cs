using LearningLantern.ApiGateway.Classroom.Commands;
using LearningLantern.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.ApiGateway.Controllers;

[Route("/api/ahrdmgl")]
public class ClassroomController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public ClassroomController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> AddClassroom(string classroomId)
    {
        var response = await _mediator.Send(new CreateNewClassroomCommand {ClassroomId = classroomId});
        return ResponseToIActionResult(response);
    }
}