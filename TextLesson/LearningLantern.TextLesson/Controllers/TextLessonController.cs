using Azure.Storage.Blobs.Models;
using LearningLantern.Common;
using LearningLantern.Common.Response;
using LearningLantern.TextLesson.Data.Models;
using LearningLantern.TextLesson.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.TextLesson.Controllers;

//[Authorize]
[Route("api/v1/[controller]")]
public class TextLessonController : ApiControllerBase
{
    private readonly ITextLessonRepository _textLessonRepository;

    public TextLessonController(ITextLessonRepository textLessonRepository)
    {
        _textLessonRepository = textLessonRepository;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Response<TextLessonDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] string title)
    {
        var response = await _textLessonRepository.AddAsync(title);
        return ResponseToIActionResult(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Response<BlobDownloadInfo>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<BlobDownloadInfo>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<BlobDownloadInfo>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromForm] AddTextLessonDTO textLesson)
    {
        var response = await _textLessonRepository.AddAsync(textLesson);

        if (!response.Succeeded) return ResponseToIActionResult(response);

        var blobDownloadInfo = response.Data!;
        return new FileStreamResult(blobDownloadInfo.Content, blobDownloadInfo.ContentType);
    }

    [HttpGet("{textLessonId:int}")]
    [ProducesResponseType(typeof(Response<BlobDownloadInfo>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<BlobDownloadInfo>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<BlobDownloadInfo>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get([FromRoute] int textLessonId)
    {
        var response = await _textLessonRepository.GetAsync(textLessonId);

        if (!response.Succeeded) return ResponseToIActionResult(response);

        var blobDownloadInfo = response.Data!;
        return new FileStreamResult(blobDownloadInfo.Content, blobDownloadInfo.ContentType);
    }

    [HttpDelete("{textLessonId:int}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int textLessonId)
    {
        var response = await _textLessonRepository.RemoveAsync(textLessonId);
        return ResponseToIActionResult(response);
    }
}