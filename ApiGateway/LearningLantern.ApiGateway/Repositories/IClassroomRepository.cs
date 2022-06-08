using LearningLantern.ApiGateway.Data.DTOs;
using LearningLantern.Common.Response;

namespace LearningLantern.ApiGateway.Repositories;

public interface IClassroomRepository
{
    public Task<Response<IEnumerable<ClassroomDTO>>> GetAllAsync();
    public Task<Response<IEnumerable<ClassroomDTO>>> GetAsync(string userId);
    public Task<Response<ClassroomDTO>> AddAsync(AddClassroomDTO addClassroomDTO);
    public Task<Response> UpdateAsync(ClassroomDTO classroomDTO);
    public Task<Response> RemoveAsync(int classroomId);
    public Task<Response> AddUserAsync(int classroomId, string userId);
    public Task<Response> RemoveUserAsync(int classroomId, string userId);
}