using LearningLantern.Common;
using LearningLantern.Common.Response;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.Video;

public abstract class VideoApiControllerBase : ApiControllerBase
{
    protected new IActionResult ResponseToIActionResult<T>(Response<T> response)
    {
        if (response.Succeeded) return Ok(response.Data);

        if (response.Errors!.Any(x => x.StatusCode == StatusCodes.Status404NotFound))
            return NotFound(response.Errors.ToJsonStringContent());

        return BadRequest(response.Errors.ToJsonStringContent());
    }
}