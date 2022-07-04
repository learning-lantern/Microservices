using Azure.Storage.Blobs.Models;
using LearningLantern.Common.Response;
using LearningLantern.TextLesson.Data.Models;

namespace LearningLantern.TextLesson.Repositories;

public interface ITextLessonRepository
{
    Task<Response<TextLessonDTO>> AddAsync(string title);
    Task<Response<BlobDownloadInfo>> AddAsync(AddTextLessonDTO textLesson);
    Task<Response<BlobDownloadInfo>> GetAsync(int textLessonId);
    Task<Response> RemoveAsync(int textLessonId);
}