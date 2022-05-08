using LearningLantern.Common.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningLantern.Common;

[ApiController]
public class ApiControllerBase : ControllerBase
{
    protected IActionResult ResponseToIActionResult(Response.Response response)
    {
        if (response.Succeeded) return Ok(response.ToJsonStringContent());

        if (response.Errors!.Any(x => x.StatusCode == StatusCodes.Status404NotFound))
            return NotFound(response.ToJsonStringContent());

        return BadRequest(response.ToJsonStringContent());
    }

    protected IActionResult ResponseToIActionResult<T>(Response<T> response)
    {
        if (response.Succeeded) return Ok(response.ToJsonStringContent());

        if (response.Errors!.Any(x => x.StatusCode == StatusCodes.Status404NotFound))
            return NotFound(response.ToJsonStringContent());

        return BadRequest(response.ToJsonStringContent());
    }
}