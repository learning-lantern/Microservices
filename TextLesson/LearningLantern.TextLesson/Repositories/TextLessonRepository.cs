using AutoMapper;
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

    public async Task<Response<TextLessonDTO>> AddAsync(string userId, AddTextLessonDTO textLesson)
    {
        var textLessonModel = new TextLessonModel(userId, textLesson)
        {
            BlobName = Guid.NewGuid().ToString()
        };

        var result = await _blobServiceClient.UploadBlobAsync(textLessonModel.BlobName, textLesson.File);

        if (result == false)
            return ResponseFactory.Fail<TextLessonDTO>(ErrorsList.CantUploadFile());

        var entity = await _context.TextLessons.AddAsync(textLessonModel);

        var saveResult = await _context.SaveChangesAsync();
        return saveResult != 0
            ? ResponseFactory.Ok(_mapper.Map<TextLessonDTO>(entity.Entity))
            : ResponseFactory.Fail<TextLessonDTO>();
    }

    public async Task<Response<BlobDownloadInfo>> GetAsync(int textLessonId)
    {
        var textLesson = await _context.TextLessons.FindAsync(textLessonId);
        if (textLesson is null)
            return ResponseFactory.Fail<BlobDownloadInfo>(ErrorsList.TextLessonNotFound(textLessonId));

        var result = await _blobServiceClient.DownloadBlobAsync(textLesson.BlobName);

        return result is null
            ? ResponseFactory.Fail<BlobDownloadInfo>(ErrorsList.CantDownloadFile())
            : ResponseFactory.Ok(result);
    }

    public async Task<Response> UpdateAsync(TextLessonModel textLesson)
    {
        var textLessonModel = await _context.TextLessons.FindAsync(textLesson.Id);

        if (textLessonModel == null) return ResponseFactory.Fail();

        textLessonModel.Name = textLesson.Name;

        var result = await _context.SaveChangesAsync() != 0;
        return result ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }

    public async Task<Response> RemoveAsync(int textLessonId)
    {
        var textLesson = await _context.TextLessons.FindAsync(textLessonId);

        if (textLesson == null) return ResponseFactory.Ok();

        await _blobServiceClient.DeleteBlobAsync(textLesson.BlobName);

        _context.TextLessons.Remove(textLesson);

        var result = await _context.SaveChangesAsync() != 0;

        return result ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }
}