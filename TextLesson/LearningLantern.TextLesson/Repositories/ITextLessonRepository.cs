using Azure.Storage.Blobs.Models;
using LearningLantern.Common.Response;
using LearningLantern.TextLesson.Data.Models;

namespace LearningLantern.TextLesson.Repositories;

public interface ITextLessonRepository
{
    Task<Response<TextLessonDTO>> AddAsync(string title, int classroomId);
    Task<Response<IFormFile>> AddAsync(AddTextLessonDTO textLesson);
    Task<Response<BlobDownloadInfo>> GetAsync(int textLessonId);
    Task<Response<List<TextLessonDTO>>> GetTextLessonsAsync(int classroomId);
    Task<Response> RemoveAsync(int textLessonId);
}