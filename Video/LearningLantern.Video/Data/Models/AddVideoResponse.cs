namespace LearningLantern.Video.Data.Models;

public class AddVideoResponse
{
    public AddVideoResponse(VideoModel video, string tempId)
    {
        Video = video;
        TempId = tempId;
    }

    public VideoModel Video { get; }
    public string TempId { get; }
}