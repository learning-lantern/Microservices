using LearningLantern.Video.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.Video.Data;

public class VideoContext : DbContext, IVideoContext
{
    public VideoContext(DbContextOptions<VideoContext> option) : base(option)
    {
    }

    public DbSet<VideoModel> Videos { get; set; } = null!;
}