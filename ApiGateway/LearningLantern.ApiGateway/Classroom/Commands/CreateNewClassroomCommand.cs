using LearningLantern.ApiGateway.Data;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.Common.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.ApiGateway.Classroom.Commands;

public class CreateNewClassroomCommand : IRequest<Response>
{
    public string ClassroomId { get; set; }
}

public class CreateNewClassroomCommandHandler : IRequestHandler<CreateNewClassroomCommand, Response>
{
    private readonly ILearningLanternContext _context;

    public CreateNewClassroomCommandHandler(ILearningLanternContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(CreateNewClassroomCommand request, CancellationToken cancellationToken)
    {
        var classroom = new ClassroomModel
        {
            Id = request.ClassroomId
        };
        var tmp = await _context.Classrooms.Where(x => x.Id == request.ClassroomId)
            .FirstOrDefaultAsync(cancellationToken);

        if (tmp is not null) return ResponseFactory.Ok();
        
        await _context.Classrooms.AddAsync(classroom, cancellationToken);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result != 0 ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }
}