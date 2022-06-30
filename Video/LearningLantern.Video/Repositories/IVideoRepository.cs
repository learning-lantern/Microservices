using Azure.Storage.Blobs.Models;
using LearningLantern.Common.Response;
using LearningLantern.Video.Data.Models;

namespace LearningLantern.Video.Repositories;

public interface IVideoRepository
{
    Task<Response> AddAsync(string userId, AddVideoDTO video);
    Task<Response> GetAsync(int videoId);
    Task<Response> UpdateAsync(VideoModel video);
    Task<Response> RemoveAsync(int videoId);
}