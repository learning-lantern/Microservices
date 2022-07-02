using LearningLantern.TextLesson.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TextLesson.Data;

public interface ITextLessonContext
{
    DbSet<TextLessonModel> TextLessons { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}