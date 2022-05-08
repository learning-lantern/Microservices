using LearningLantern.ApiGateway.Data.DTOs;

namespace LearningLantern.ApiGateway.Repositories;

public interface IClassroomRepository
{
    public Task<List<ClassroomDTO>> GetAsync(string userId);

    public Task<int?> AddAsync(AddClassroomDTO addClassroomDTO, string userId);

    public Task<bool?> AddUserAsync(int classroomId, string requestUserId, string userId);

    public Task<bool?> UpdateAsync(ClassroomDTO classroomDTO, string userId);

    public Task<bool?> RemoveUserAsync(int classroomId, string requestUserId, string userId);

    public Task<bool?> RemoveAsync(int classroomId, string userId);
}