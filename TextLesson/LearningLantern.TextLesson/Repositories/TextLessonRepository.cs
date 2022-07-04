using AutoMapper;
using LearningLantern.Common.Response;
using LearningLantern.TextLesson.Data;
using LearningLantern.TextLesson.Data.Models;
using LearningLantern.TextLesson.Utility;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TextLesson.Repositories;

public class TextLessonRepository : ITextLessonRepository
{
    private readonly ITextLessonContext _context;
    private readonly ILogger<TextLessonRepository> _logger;
    private readonly IMapper _mapper;

    public TextLessonRepository(
        ITextLessonContext context, IMapper mapper, ILogger<TextLessonRepository> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Response<TextLessonDTO>> AddAsync(AddTextLessonDTO addTextLessonDTO)
    {
        var textLessonModel = new TextLessonModel
        {
            Title = addTextLessonDTO.Title,
            ClassroomId = addTextLessonDTO.ClassroomId
        };

        var entity = await _context.TextLessons.AddAsync(textLessonModel);

        var saveResult = await _context.SaveChangesAsync();

        return saveResult != 0
            ? ResponseFactory.Ok(_mapper.Map<TextLessonDTO>(entity.Entity))
            : ResponseFactory.Fail<TextLessonDTO>();
    }

    public async Task<Response<int>> UpdateAsync(UpdateTextLessonDTO updateTextLessonDTO)
    {
        var textLessonModel = await _context.TextLessons.FirstOrDefaultAsync(x => x.Id == updateTextLessonDTO.Id);

        if (textLessonModel is null)
            return ResponseFactory.Fail<int>(ErrorsList.TextLessonNotFound(updateTextLessonDTO.Id));

        textLessonModel.HtmlBody = updateTextLessonDTO.HtmlBody;
        _context.TextLessons.Update(textLessonModel);
        var result = await _context.SaveChangesAsync();
        return result != 0 ? ResponseFactory.Ok(textLessonModel.Id) : ResponseFactory.Fail<int>();
    }

    public async Task<Response<TextLessonDTO>> GetAsync(int textLessonId)
    {
        var textLesson = await _context.TextLessons.FindAsync(textLessonId);

        return textLesson is null
            ? ResponseFactory.Fail<TextLessonDTO>(ErrorsList.TextLessonNotFound(textLessonId))
            : ResponseFactory.Ok(_mapper.Map<TextLessonDTO>(textLesson));
    }

    public async Task<Response<List<TextLessonDTO>>> GetTextLessonsAsync(string classroomId) => ResponseFactory.Ok(
        await _context.TextLessons.AsNoTracking().Where(textlesson => textlesson.ClassroomId == classroomId)
            .Select(textLesson => _mapper.Map<TextLessonDTO>(textLesson)).ToListAsync());

    public async Task<Response> RemoveAsync(int textLessonId)
    {
        var textLesson = await _context.TextLessons.FindAsync(textLessonId);

        if (textLesson == null) return ResponseFactory.Ok();

        _context.TextLessons.Remove(textLesson);

        var result = await _context.SaveChangesAsync() != 0;

        return result ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }
}