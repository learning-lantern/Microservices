using AutoMapper;
using Azure.Storage.Blobs.Models;
using LearningLantern.AzureBlobStorage;
using LearningLantern.Common.Response;
using LearningLantern.Video.Data;
using LearningLantern.Video.Data.Models;
using LearningLantern.Video.Utility;

namespace LearningLantern.Video.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly IBlobService _blobServiceClient;
    private readonly IVideoContext _context;
    private readonly ILogger<VideoRepository> _logger;
    private readonly IMapper _mapper;

    public VideoRepository(
        IVideoContext context, IBlobService blobServiceClient, IMapper mapper, ILogger<VideoRepository> logger)
    {
        _context = context;
        _blobServiceClient = blobServiceClient;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Response<VideoDTO>> AddAsync(string userId, AddVideoDTO video)
    {
        var videoModel = new VideoModel(userId, video)
        {
            BlobName = Guid.NewGuid().ToString()
        };

        var result = await _blobServiceClient.UploadBlobAsync(videoModel.BlobName, video.File);

        if (result == false)
            return ResponseFactory.Fail<VideoDTO>(ErrorsList.CantUploadFile());

        var entity = await _context.Videos.AddAsync(videoModel);

        var saveResult = await _context.SaveChangesAsync();
        return saveResult != 0
            ? ResponseFactory.Ok(_mapper.Map<VideoDTO>(videoModel))
            : ResponseFactory.Fail<VideoDTO>();
    }

    public async Task<Response<BlobDownloadInfo>> GetAsync(int videoId)
    {
        var video = await _context.Videos.FindAsync(videoId);
        if (video is null)
            return ResponseFactory.Fail<BlobDownloadInfo>(ErrorsList.VideoNotFound(videoId));

        var result = await _blobServiceClient.DownloadBlobAsync(video.BlobName);

        return result is null
            ? ResponseFactory.Fail<BlobDownloadInfo>(ErrorsList.CantDownloadFile())
            : ResponseFactory.Ok(result);
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

        await _blobServiceClient.DeleteBlobAsync(video.BlobName);

        _context.Videos.Remove(video);

        var result = await _context.SaveChangesAsync() != 0;

        return result ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }
}