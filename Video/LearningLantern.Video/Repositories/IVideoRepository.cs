using Azure.Storage.Blobs.Models;
using LearningLantern.Common.Response;
using LearningLantern.Video.Data.Models;

namespace LearningLantern.Video.Repositories;

public interface IVideoRepository
{
    Task<Response<VideoDTO>> AddAsync(AddVideoDTO video);
    Task<Response<VideoDTO>> GetAsync(int videoId);
    Task<Response> RemoveAsync(int videoId);
}