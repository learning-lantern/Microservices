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
    [ProducesResponseType(typeof(TextLessonDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] AddTextLessonDTO addTextLessonDTO)
    {
        var response = await _textLessonRepository.AddAsync(addTextLessonDTO);
        return ResponseToIActionResult(response);
    }

    [HttpPut("{textLessonId:int}")]
    [ProducesResponseType(typeof(IFormFile), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] int textLessonId, [FromBody] string htmlBody)
    {
        var response = await _textLessonRepository.UpdateAsync(textLessonId, htmlBody);
        return ResponseToIActionResult(response);
    }

    [HttpGet("{textLessonId:int}")]
    [ProducesResponseType(typeof(BlobDownloadInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get([FromRoute] int textLessonId)
    {
        var response = await _textLessonRepository.GetAsync(textLessonId);
        return ResponseToIActionResult(response);
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
    public async Task<IActionResult> Remove([FromRoute] int textLessonId)
    {
        var response = await _textLessonRepository.RemoveAsync(textLessonId);
        return ResponseToIActionResult(response);
    }
}