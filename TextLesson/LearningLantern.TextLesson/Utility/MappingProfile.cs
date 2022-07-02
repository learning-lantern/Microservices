using AutoMapper;
using LearningLantern.TextLesson.Data.Models;

namespace LearningLantern.TextLesson.Utility;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TextLessonModel, TextLessonDTO>();
    }
}