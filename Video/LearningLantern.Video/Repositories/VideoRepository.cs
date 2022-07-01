using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LearningLantern.Common.Response;
using LearningLantern.Video.Data;
using LearningLantern.Video.Data.Models;
using LearningLantern.Video.Utility;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.Video.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly IVideoContext _context;
    private readonly BlobServiceClient _blobServiceClient;

    public VideoRepository(IVideoContext context, BlobServiceClient blobServiceClient)
    {
        _context = context;
        _blobServiceClient = blobServiceClient;
    }

    public async Task<Response<VideoModel>> AddAsync(string userId, AddVideoDTO video)
    {
        var videoEntity = await _context.Videos.AddAsync(new VideoModel(userId, video));
        var result = await _context.SaveChangesAsync();

        var blobContainerClient = _blobServiceClient.GetBlobContainerClient("videos");
        var blobClient = blobContainerClient.GetBlobClient(videoEntity.Entity.Id.ToString());

        await blobClient.UploadAsync(video.Path, new BlobHttpHeaders { ContentType = video.Path.GetContentType() });

        return result == 0
            ? ResponseFactory.Fail<VideoModel>()
            : ResponseFactory.Ok(videoEntity.Entity);
    }

    public async Task<Response<FileStreamResult>> GetAsync(int videoId)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient("videos");
        var blobClient = blobContainerClient.GetBlobClient(videoId.ToString());

        var blobDownloadInfo = (await blobClient.DownloadAsync()).Value;

        return blobDownloadInfo is null
                ? ResponseFactory.Fail<FileStreamResult>(ErrorsList.VideoNotFound(videoId))
                : ResponseFactory.Ok(new FileStreamResult(blobDownloadInfo.Content, blobDownloadInfo.ContentType));
    }

    // public async Task<Response> UpdateAsync(VideoModel video)
    // {
    //     var videoModel = await _context.Videos.FindAsync(video.Id);

    //     if (videoModel == null) return ResponseFactory.Fail();

    //     videoModel.Name = video.Name;

    //     var result = await _context.SaveChangesAsync() != 0;
    //     return result ? ResponseFactory.Ok() : ResponseFactory.Fail();
    // }

    // public async Task<Response> RemoveAsync(int videoId)
    // {
    //     var video = await _context.Videos.FindAsync(videoId);

    //     if (video == null) return ResponseFactory.Ok();

    //     _context.Videos.Remove(video);

    //     var result = await _context.SaveChangesAsync() != 0;

    //     var containerClient = _blobServiceClient.GetBlobContainerClient("videos");
    //     var blobClient = containerClient.GetBlobClient(videoId.ToString());
    //     await blobClient.DeleteIfExistsAsync();

    //     return result ? ResponseFactory.Ok() : ResponseFactory.Fail();
    // }
}