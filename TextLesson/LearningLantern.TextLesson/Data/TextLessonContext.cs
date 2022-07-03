using LearningLantern.TextLesson.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.TextLesson.Data;

public class TextLessonContext : DbContext, ITextLessonContext
{
    public TextLessonContext(DbContextOptions option) : base(option)
    {
        
    }

    public DbSet<TextLessonModel> TextLessons { get; set; } = null!;
}