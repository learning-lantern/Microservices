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

    [HttpPost("{title}/Classroom/{classroomId}")]
    [ProducesResponseType(typeof(TextLessonDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromRoute] string title, [FromRoute] string classroomId)
    {
        var response = await _textLessonRepository.AddAsync(title, classroomId);
        return ResponseToIActionResult(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(IFormFile), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromForm] AddTextLessonDTO textLesson)
    {
        var response = await _textLessonRepository.AddAsync(textLesson);
        return response.Succeeded
            ? new FileStreamResult(response.Data!.OpenReadStream(), response.Data.ContentType)
            : ResponseToIActionResult(response);
    }

    [HttpGet("{textLessonId}")]
    [ProducesResponseType(typeof(BlobDownloadInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get([FromRoute] string textLessonId)
    {
        var response = await _textLessonRepository.GetAsync(textLessonId);

        if (!response.Succeeded) return ResponseToIActionResult(response);

        var blobDownloadInfo = response.Data!;
        return new FileStreamResult(blobDownloadInfo.Content, blobDownloadInfo.ContentType);
    }

    [HttpGet("Classroom/{classroomId}")]
    [ProducesResponseType(typeof(List<TextLessonDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTextLessons([FromRoute] string classroomId)
    {
        var response = await _textLessonRepository.GetTextLessonsAsync(classroomId);
        return ResponseToIActionResult(response);
    }

    [HttpDelete("{textLessonId}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] string textLessonId)
    {
        var response = await _textLessonRepository.RemoveAsync(textLessonId);
        return ResponseToIActionResult(response);
    }
}