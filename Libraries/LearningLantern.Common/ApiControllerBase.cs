using LearningLantern.Common.Extensions;
using LearningLantern.Common.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.Common;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult ResponseToIActionResult(Response.Response response)
    {
        if (response.Succeeded) return Ok();

        if (response.Errors!.Any(x => x.StatusCode == StatusCodes.Status404NotFound))
            return NotFound(response.Errors.ToJsonStringContent());

        return BadRequest(response.Errors.ToJsonStringContent());
    }

    protected IActionResult ResponseToIActionResult<T>(Response<T> response)
    {
        if (response.Succeeded) return Ok(response.Data.ToJsonStringContent());

        if (response.Errors!.Any(x => x.StatusCode == StatusCodes.Status404NotFound))
            return NotFound(response.Errors.ToJsonStringContent());

        return BadRequest(response.Errors.ToJsonStringContent());
    }
}