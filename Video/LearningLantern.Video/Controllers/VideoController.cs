using LearningLantern.Common;
using LearningLantern.Common.Response;
using LearningLantern.Common.Services;
using LearningLantern.Video.Data.Models;
using LearningLantern.Video.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.Video.Controllers;

//[Authorize]
[Route("api/v1/[controller]")]
public class VideoController : ApiControllerBase
{
    private readonly IVideoRepository _videoRepository;

    public VideoController(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<int> Add([FromForm] AddVideoDTO video)
    {
        var response = await _videoRepository.AddAsync(video);
        return response.Data is not null ? response.Data.Id : 0;
    }

    [HttpGet("{videoId:int}")]
    [ProducesResponseType(typeof(Response<VideoDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] int videoId)
    {
        var response = await _videoRepository.GetAsync(videoId);
        return ResponseToIActionResult(response);
    }

    [HttpDelete("{videoId:int}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int videoId)
    {
        var response = await _videoRepository.RemoveAsync(videoId);
        return ResponseToIActionResult(response);
    }
}