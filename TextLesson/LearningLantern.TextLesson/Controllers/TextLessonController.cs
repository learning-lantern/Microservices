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
    private readonly ITextLessonRepository _textLessonRepository;

    public TextLessonController(ITextLessonRepository textLessonRepository)
    {
        _textLessonRepository = textLessonRepository;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Response<TextLessonDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromForm] AddTextLessonDTO textLesson)
    {
        var response = await _textLessonRepository.AddAsync(textLesson);
        return ResponseToIActionResult(response);
    }

    [HttpGet("{textLessonId:int}")]
    [ProducesResponseType(typeof(Response<TextLessonDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] int textLessonId)
    {
        var response = await _textLessonRepository.GetAsync(textLessonId);
        return ResponseToIActionResult(response);
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