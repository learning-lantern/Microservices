using AutoMapper;
using LearningLantern.Video.Data.Models;

namespace LearningLantern.Video.Utility;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<VideoModel, VideoDTO>();
    }
}