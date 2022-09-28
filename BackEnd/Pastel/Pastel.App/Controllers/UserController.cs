using Microsoft.AspNetCore.Mvc;
using Pastel.Domain.Command;
using Pastel.Domain.Dto;
using Pastel.Handles.Interfaces;

namespace Pastel.App.Controllers
{
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody]CreateUserCommand command, 
            [FromServices] ICreateUserCommandHandle handle)
        {
            try
            {
                if (!command.IsValid())
                    return BadRequest(command.Errors());

                var result = await handle.Create(command);

                if (result.Errors.Count > 0)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception error)
            {
                var message = $"{error.InnerException}\n " +
                    $"{error.Message} \n " +
                    $"{error.StackTrace}";

                _logger.LogError(message);

                return BadRequest(message);
            }
        }

        [HttpPost("uploadphoto")]
        public async Task<ActionResult> UploadImageAsync(Guid userId, 
            [FromServices]IImageHandle handle)
        {
            var result = new ResultDto();
            try
            {
                var file = Request.Form.Files[0];

                if(file == null)
                    return BadRequest("Não foi recebida o arquivo");

                if(file.ContentType.Contains("image/jpeg") || file.ContentType.Contains("image/png"))
                {
                    result = await handle.ImageIngestionAsync(file, userId);
                }

                if (result.Errors.Count > 0)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception error)
            {
                var message = $"{error.InnerException}\n " +
                    $"{error.Message} \n " +
                    $"{error.StackTrace}";

                _logger.LogError(message);

                return BadRequest(message);
            }
        }

        [HttpGet("getphoto")]
        public async Task<FileStreamResult> GetPhoto(Guid userId, [FromServices]IImageHandle handle)
        {
            return await handle.GetPhoto(userId);
        }
    }
}
