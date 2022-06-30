using FluentAssertions;
using Xunit;

namespace LearningLantern.ApiGateway.Tests;

public class UnitTest1 : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UnitTest1(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }


    [Theory]
    [InlineData("/swagger/index.html")]
    public async Task Given_endpoints_should_return_success_HTTP_statusCode(string endpoint)
    {
        //ARRANGE
        //ACT
        //ASSERT
        var response = await _client.GetAsync(endpoint);
        response.Should().BeSuccessful();
    }
}