using AutoMapper;
using LearningLantern.ApiGateway.Classroom.DTOs;
using LearningLantern.ApiGateway.Classroom.Models;
using LearningLantern.ApiGateway.Data;
using LearningLantern.ApiGateway.Users.Queries;
using LearningLantern.ApiGateway.Utility;
using LearningLantern.Common.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.ApiGateway.Classroom.Repositories;

public class ClassroomRepository : IClassroomRepository
{
    private readonly ILearningLanternContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ClassroomRepository(ILearningLanternContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Response<IEnumerable<ClassroomDTO>>> GetAsync(string userId)
    {
        var result = await _context.ClassroomUsers
            .Where(classroomUser => classroomUser.UserId == userId)
            .Select(classroomUser => _mapper.Map<ClassroomDTO>(classroomUser.Classroom)).ToListAsync();
        return ResponseFactory.Ok<IEnumerable<ClassroomDTO>>(result);
    }

    public async Task<Response<IEnumerable<ClassroomDTO>>> GetAllAsync()
    {
        var result = await _context.Classrooms
            .Select(classroom => _mapper.Map<ClassroomDTO>(classroom)).ToListAsync();
        return ResponseFactory.Ok<IEnumerable<ClassroomDTO>>(result);
    }

    public async Task<Response<ClassroomDTO>> AddAsync(AddClassroomDTO addClassroomDTO)
    {
        var tmpClassroom = _mapper.Map<ClassroomModel>(addClassroomDTO);
        var classroom = await _context.Classrooms.AddAsync(tmpClassroom);

        return await _context.SaveChangesAsync() != 0
            ? ResponseFactory.Ok(_mapper.Map<ClassroomDTO>(classroom.Entity))
            : ResponseFactory.Fail<ClassroomDTO>();
    }

    public async Task<Response> AddUserAsync(int classroomId, string userId)
    {
        var user = (await _mediator.Send(new GetUserByIdQuery {UserId = userId})).Data;
        var classroom = await GetClassroomById(classroomId);

        if (user is not null && classroom is not null)
        {
            await _context.ClassroomUsers.AddAsync(new ClassroomUserModel(classroomId, userId));
            var result = await _context.SaveChangesAsync();
            if (result != 0) return ResponseFactory.Ok();
        }

        var errors = new List<Error>();
        if (user is null) errors.Add(ErrorsList.UserIdNotFound(userId));
        if (classroom is null) errors.Add(ErrorsList.ClassroomIdNotFound(classroomId));

        return ResponseFactory.Fail(errors);
    }

    public async Task<Response> UpdateAsync(ClassroomDTO classroomDTO)
    {
        var classroom = await GetClassroomById(classroomDTO.Id);

        if (classroom == null) return ResponseFactory.Fail(ErrorsList.ClassroomIdNotFound(classroomDTO.Id));

        classroom.Update(classroomDTO);
        _context.Classrooms.Update(classroom);

        return await _context.SaveChangesAsync() != 0 ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }

    public async Task<Response> RemoveUserAsync(int classroomId, string userId)
    {
        var classroomUser = await _context.ClassroomUsers
            .FirstOrDefaultAsync(
                classroomUser => classroomUser.UserId == userId && classroomUser.ClassroomId == classroomId);

        if (classroomUser is null) return ResponseFactory.Ok();

        _context.ClassroomUsers.Remove(classroomUser);

        return await _context.SaveChangesAsync() != 0 ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }

    public async Task<Response> RemoveAsync(int classroomId)
    {
        var classroom = await GetClassroomById(classroomId);

        if (classroom is null) return ResponseFactory.Fail(ErrorsList.ClassroomIdNotFound(classroomId));

        _context.Classrooms.Remove(classroom);
        return await _context.SaveChangesAsync() != 0 ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }

    public Task<ClassroomModel?> GetClassroomById(int classroomId)
        => _context.Classrooms.FirstOrDefaultAsync(classroom => classroom.Id == classroomId);
}