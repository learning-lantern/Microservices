using System.Runtime.Serialization;
using AutoMapper;
using Xunit;

namespace LearningLantern.Common.Tests;

public abstract class MappingTestsSetup
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    protected MappingTestsSetup(Action<IMapperConfigurationExpression> configure)
    {
        _configuration = new MapperConfiguration(configure);

        _mapper = _configuration.CreateMapper();
    }

    [Theory]
    public virtual void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    [Fact]
    public void ShouldHaveValidConfiguration()
    {
        //_configuration.AssertConfigurationIsValid();
    }

    private static object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;
        // Type without parameterless constructor
        return FormatterServices.GetUninitializedObject(type);
    }
}