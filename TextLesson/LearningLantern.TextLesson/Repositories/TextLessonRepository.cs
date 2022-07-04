using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LearningLantern.AzureBlobStorage;
using LearningLantern.Common.Response;
using LearningLantern.TextLesson.Data;
using LearningLantern.TextLesson.Data.Models;
using LearningLantern.TextLesson.Utility;

namespace LearningLantern.TextLesson.Repositories;

public class TextLessonRepository : ITextLessonRepository
{
    private readonly IBlobService _blobServiceClient;
    private readonly ITextLessonContext _context;
    private readonly ILogger<TextLessonRepository> _logger;
    private readonly IMapper _mapper;

    public TextLessonRepository(
        ITextLessonContext context, IBlobService blobServiceClient, IMapper mapper, ILogger<TextLessonRepository> logger)
    {
        _context = context;
        _blobServiceClient = blobServiceClient;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Response<TextLessonDTO>> AddAsync(string title)
    {
        var textLessonModel = new TextLessonModel
        {
            Title = title,
            BlobName = Guid.NewGuid().ToString()
        };

        var entity = await _context.TextLessons.AddAsync(textLessonModel);

        var saveResult = await _context.SaveChangesAsync();

        return saveResult != 0
            ? ResponseFactory.Ok(_mapper.Map<TextLessonDTO>(entity.Entity))
            : ResponseFactory.Fail<TextLessonDTO>();
    }

    public async Task<Response<BlobDownloadInfo>> AddAsync(AddTextLessonDTO textLesson)
    {
        var textLessonModel = await _context.TextLessons.FindAsync(Convert.ToInt32(textLesson.Id));
        
        if (textLessonModel is null)
            return ResponseFactory.Fail<BlobDownloadInfo>(ErrorsList.TextLessonNotFound(Convert.ToInt32(textLesson.Id)));

        var result = await _blobServiceClient.UploadBlobAsync(textLessonModel.BlobName, textLesson.File);
        
        if (result == string.Empty)
            return ResponseFactory.Fail<BlobDownloadInfo>(ErrorsList.CantUploadFile());

        var file = await _blobServiceClient.DownloadBlobAsync(textLessonModel.BlobName);

        return file is null
            ? ResponseFactory.Ok(file)
            : ResponseFactory.Fail<BlobDownloadInfo>();
    }

    public async Task<Response<BlobDownloadInfo>> GetAsync(int textLessonId)
    {
        var textLesson = await _context.TextLessons.FindAsync(textLessonId);

        if (textLesson is null)
            return ResponseFactory.Fail<BlobDownloadInfo>(ErrorsList.TextLessonNotFound(textLessonId));

        var file = await _blobServiceClient.DownloadBlobAsync(textLesson.BlobName);

        if (file is null)
            return ResponseFactory.Fail<BlobDownloadInfo>(ErrorsList.CantDownloadFile());

        return ResponseFactory.Ok(file);
    }

    public async Task<Response> RemoveAsync(int textLessonId)
    {
        var textLesson = await _context.TextLessons.FindAsync(textLessonId);

        if (textLesson == null) return ResponseFactory.Ok();

        await  _blobServiceClient.DeleteBlobAsync(textLesson.BlobName);

        _context.TextLessons.Remove(textLesson);

        var result = await _context.SaveChangesAsync() != 0;

        return result ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }
}