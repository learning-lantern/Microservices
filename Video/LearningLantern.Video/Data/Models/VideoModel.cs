using System.ComponentModel.DataAnnotations;
using LearningLantern.Video.Data.Models;

public class VideoModel : VideoProperties
{
    [Required] public string InstructorId { get; set; }
    [Required] public string ClassRoomId { get; set; }

    public void Update(VideoProperties videoProperties)
    {
        Path = videoProperties.Path;
        Name = videoProperties.Name;
        Type = videoProperties.Type;
        Discription = videoProperties.Discription;
    }
}