using AutoMapper;
using Azure.Storage.Blobs;
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

    public async Task<Response<TextLessonDTO>> AddAsync(AddTextLessonDTO textLesson)
    {
        var textLessonModel = new TextLessonModel()
        {
            BlobName = Guid.NewGuid().ToString(),
            QuizList = textLesson.QuizList
        };

        var result = await _blobServiceClient.UploadBlobAsync(textLessonModel.BlobName, textLesson.File);
        
        if (result == string.Empty)
            return ResponseFactory.Fail<TextLessonDTO>(ErrorsList.CantUploadFile());
        
        textLessonModel.Path = result;

        var entity = await _context.TextLessons.AddAsync(textLessonModel);

        var saveResult = await _context.SaveChangesAsync();

        return saveResult != 0
            ? ResponseFactory.Ok(_mapper.Map<TextLessonDTO>(entity.Entity))
            : ResponseFactory.Fail<TextLessonDTO>();
    }

    public async Task<Response<TextLessonDTO>> GetAsync(int textLessonId)
    {
        var textLesson = await _context.TextLessons.FindAsync(textLessonId);

        if (textLesson is null)
            return ResponseFactory.Fail<TextLessonDTO>(ErrorsList.TextLessonNotFound(textLessonId));

        return ResponseFactory.Ok(_mapper.Map<TextLessonDTO>(textLesson));
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