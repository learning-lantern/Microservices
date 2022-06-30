using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using LearningLantern.Common;
using LearningLantern.Common.Response;
using LearningLantern.Common.Services;
using LearningLantern.Video.Data.Models;
using LearningLantern.Video.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Video.Controllers
{
    //[Authorize]
    [Route("api/v1/[controller]")]
    public class VideoController : ApiControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IVideoRepository _videoRepository;

        public VideoController(IVideoRepository videoRepository, ICurrentUserService currentUserService)
        {
            _videoRepository = videoRepository;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] AddVideoDTO video)
        {
            var userId = "test";
            var response = await _videoRepository.AddAsync(userId!, video);
            return ResponseToIActionResult(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<BlobDownloadInfo>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] int videoId)
        {
            var response = await _videoRepository.GetAsync(videoId);
            return ResponseToIActionResult(response);
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
}
