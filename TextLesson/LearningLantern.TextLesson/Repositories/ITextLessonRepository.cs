using Azure.Storage.Blobs.Models;
using LearningLantern.Common.Response;
using LearningLantern.TextLesson.Data.Models;

namespace LearningLantern.TextLesson.Repositories;

public interface ITextLessonRepository
{
    Task<Response<TextLessonDTO>> AddAsync(AddTextLessonDTO addTextLessonDTO);
    Task<Response<int>> UpdateAsync(UpdateTextLessonDTO updateTextLessonDTO);
    Task<Response<TextLessonDTO>> GetAsync(int textLessonId);
    Task<Response<List<TextLessonDTO>>> GetTextLessonsAsync(string classroomId);
    Task<Response> RemoveAsync(int textLessonId);
}