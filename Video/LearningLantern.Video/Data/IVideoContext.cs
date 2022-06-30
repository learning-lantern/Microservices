using Microsoft.EntityFrameworkCore;

namespace LearningLantern.Video.Data;

public interface IVideoContext
{
    DbSet<VideoModel> Videos { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}