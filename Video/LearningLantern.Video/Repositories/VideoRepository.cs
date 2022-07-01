using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LearningLantern.Common.Response;
using LearningLantern.Video.Data;
using LearningLantern.Video.Data.Models;
using LearningLantern.Video.Utility;

namespace LearningLantern.Video.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly BlobServiceClient _blobServiceClient;

    private readonly IVideoContext _context;

    public VideoRepository(IVideoContext context, BlobServiceClient blobServiceClient)
    {
        _context = context;
        _blobServiceClient = blobServiceClient;
    }

    public async Task<Response<VideoModel>> AddAsync(string userId, AddVideoDTO video)
    {
        var videoModel = new VideoModel(userId, video);
        await _context.Videos.AddAsync(videoModel);

        await _context.SaveChangesAsync();

        var containerClient = _blobServiceClient.GetBlobContainerClient("videos");
        var blobClient = containerClient.GetBlobClient(videoModel.Id.ToString());
        var header = new BlobHttpHeaders {ContentType = video.Path.GetContentType()};
        var response =
            await blobClient.UploadAsync(video.Path, header);
        return ResponseFactory.Ok(videoModel);
    }

    public async Task<Response<BlobDownloadInfo>> GetAsync(int videoId)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("videos");
        var blobClient = containerClient.GetBlobClient(videoId.ToString());
        var response = await blobClient.DownloadAsync();
        var blobDownloadInfo = response.Value;
        return ResponseFactory.Ok(blobDownloadInfo);
    }

    public async Task<Response> UpdateAsync(VideoModel video)
    {
        var videoModel = await _context.Videos.FindAsync(video.Id);

        if (videoModel == null) return ResponseFactory.Fail();

        videoModel.Name = video.Name;

        var result = await _context.SaveChangesAsync() != 0;
        return result ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }

    public async Task<Response> RemoveAsync(int videoId)
    {
        var video = await _context.Videos.FindAsync(videoId);

        if (video == null) return ResponseFactory.Ok();

        _context.Videos.Remove(video);

        var result = await _context.SaveChangesAsync() != 0;

        var containerClient = _blobServiceClient.GetBlobContainerClient("videos");
        var blobClient = containerClient.GetBlobClient(videoId.ToString());
        await blobClient.DeleteIfExistsAsync();

        return result ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }
}