using Azure.Storage.Blobs.Models;
using LearningLantern.Common.Response;
using LearningLantern.TextLesson.Data.Models;

namespace LearningLantern.TextLesson.Repositories;

public interface ITextLessonRepository
{
    Task<Response<TextLessonDTO>> AddAsync(AddTextLessonDTO textLesson);
    Task<Response<TextLessonDTO>> GetAsync(int textLessonId);
    Task<Response> RemoveAsync(int textLessonId);
}