using LearningLantern.AzureBlobStorage;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Response;
using LearningLantern.Video.Data;
using LearningLantern.Video.Data.Models;
using LearningLantern.Video.Utility;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LearningLantern.Video.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly IBlobService _blobServiceClient;
    private readonly IVideoContext _context;
    private readonly ILogger<VideoRepository> _logger;

    public VideoRepository(IVideoContext context, IBlobService blobServiceClient, ILogger<VideoRepository> logger)
    {
        _context = context;
        _blobServiceClient = blobServiceClient;
        _logger = logger;
    }

    public async Task<Response<VideoDTO>> AddAsync(AddVideoDTO video)
    {
        var videoModel = new VideoModel
        {
            BlobName = Guid.NewGuid().ToString()
        };
        var quizList = JsonConvert.DeserializeObject<List<VideoQuiz>>(video.QuizList);


        if (quizList is not null)
        {
            _logger.LogDebug(quizList.ToJsonStringContent());
            videoModel.QuizList.AddRange(quizList);
        }

        var result = await _blobServiceClient.UploadBlobAsync(videoModel.BlobName, video.File);

        if (result == string.Empty)
            return ResponseFactory.Fail<VideoDTO>(ErrorsList.CantUploadFile());

        videoModel.Path = result;

        var entity = await _context.Videos.AddAsync(videoModel);

        var saveResult = await _context.SaveChangesAsync();

        return saveResult != 0
            ? ResponseFactory.Ok(new VideoDTO(entity.Entity))
            : ResponseFactory.Fail<VideoDTO>();
    }

    public async Task<Response<VideoDTO>> GetAsync(int videoId)
    {
        var video = await _context.Videos.Include(x => x.QuizList)
            .FirstOrDefaultAsync(x => x.Id == videoId);

        return video is null
            ? ResponseFactory.Fail<VideoDTO>(ErrorsList.VideoNotFound(videoId))
            : ResponseFactory.Ok(new VideoDTO(video));
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