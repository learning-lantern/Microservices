using Azure.Storage.Blobs.Models;
using LearningLantern.Common;
using LearningLantern.Common.Response;
using LearningLantern.Common.Services;
using LearningLantern.TextLesson.Data.Models;
using LearningLantern.TextLesson.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.TextLesson.Controllers;

//[Authorize]
[Route("api/v1/[controller]")]
public class TextLessonController : ApiControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ITextLessonRepository _textLessonRepository;

    public TextLessonController(ITextLessonRepository textLessonRepository, ICurrentUserService currentUserService)
    {
        _textLessonRepository = textLessonRepository;
        _currentUserService = currentUserService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromForm] AddTextLessonDTO textLesson)
    {
        var userId = _currentUserService.UserId ?? "test";
        var response = await _textLessonRepository.AddAsync(userId, textLesson);
        return ResponseToIActionResult(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Response<BlobDownloadInfo>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] int textLessonId)
    {
        var response = await _textLessonRepository.GetAsync(textLessonId);

        if (!response.Succeeded) return ResponseToIActionResult(response);
        
        var blobDownloadInfo = response.Data!;
        return new FileStreamResult(blobDownloadInfo.Content, blobDownloadInfo.ContentType);
    }

    // [HttpPut("{videoId:int}")]
    // [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    // public async Task<IActionResult> Update([FromRoute] int videoId, [FromBody] VideoProperties videoProperties)
    // {
    //     try
    //     {
    //         var response = await _videoRepository.UpdateAsync(videoId, videoProperties);
    //         return Ok(response);

    //     }
    //     catch (Exception)
    //     {
    //         return BadRequest();
    //     }
    //     //return ResponseToIActionResult(response);
    // }

    // [HttpDelete("{videoId:int}")]
    // [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    // public async Task<IActionResult> Remove([FromRoute] int videoId)
    // {
    //     try
    //     {
    //         var response = await _videoRepository.RemoveAsync(videoId);
    //         return Ok(response);


    //     }
    //     catch (Exception)
    //     {
    //         return BadRequest();
    //     }
    //     //return ResponseToIActionResult(response);
    // }
}