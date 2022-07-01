using LearningLantern.Common.Response;
using LearningLantern.Video.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.Video.Repositories;

public interface IVideoRepository
{
    Task<Response<VideoModel>> AddAsync(string userId, AddVideoDTO video);
    Task<Response<FileStreamResult>> GetAsync(int videoId);
    // Task<Response> UpdateAsync(VideoModel video);
    // Task<Response> RemoveAsync(int videoId);
}