using Microsoft.EntityFrameworkCore;

namespace LearningLantern.Video.Data;

public class VideoContext : DbContext, IVideoContext
{
    public VideoContext(DbContextOptions option) : base(option)
    {
    }

    public DbSet<VideoModel> Videos { get; set; } = null!;
}