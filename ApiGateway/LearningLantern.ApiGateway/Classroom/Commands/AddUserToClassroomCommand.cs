using LearningLantern.ApiGateway.Data;
using LearningLantern.ApiGateway.Data.Models;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.ApiGateway.Classroom.Commands;

public class AddUserToClassroomCommand : IRequest<Response>
{
    public string ClassroomId { get; set; }
    public string UserId { get; set; }
}

public class AddUserToClassroomCommandHandler : IRequestHandler<AddUserToClassroomCommand, Response>
{
    private readonly ILearningLanternContext _context;
    private readonly UserManager<UserModel> _userManager;

    public AddUserToClassroomCommandHandler(ILearningLanternContext context, UserManager<UserModel> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Response> Handle(AddUserToClassroomCommand request, CancellationToken cancellationToken)
    {
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(classroom => classroom.Id == request.ClassroomId, CancellationToken.None);
        var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == request.UserId, CancellationToken.None);

        if (user is not null && classroom is not null)
        {
            await _context.ClassroomUsers.AddAsync(new ClassroomUserModel
                {UserId = request.UserId, ClassroomId = request.ClassroomId}, cancellationToken);
            var result = await _context.SaveChangesAsync(CancellationToken.None);
            return result != 0 ? ResponseFactory.Ok() : ResponseFactory.Fail();
        }

        IEnumerable<Error> errors = Array.Empty<Error>();
        if (user is null) errors = errors.Append(ErrorsList.UserIdNotFound(request.UserId));
        if (classroom is null) errors = errors.Append(ErrorsList.ClassroomIdNotFound(request.ClassroomId));
        return ResponseFactory.Fail(errors);
    }
}