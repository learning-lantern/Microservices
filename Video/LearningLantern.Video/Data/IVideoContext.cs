using LearningLantern.Video.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.Video.Data;

public interface IVideoContext
{
    public DbSet<VideoModel> Videos { get; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}