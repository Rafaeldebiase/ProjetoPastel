using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pastel.Data.Interfaces;
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

        [HttpGet("getphoto")]
        [Authorize]
        public async Task<FileStreamResult> GetPhoto(Guid userId, [FromServices] IImageHandle handle)
        {

            var result = await handle.GetPhoto(userId);
            return result.FirstOrDefault();
        }


        [HttpGet("getphone")]
        [Authorize]
        public async Task<ActionResult> GetPhone(Guid userId, [FromServices]IPhoneHandle handle)
        {

            try
            {
                var result = await handle.GetPhonesByUserId(userId);
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

        [HttpGet("getUsers")]
        [Authorize]
        public async Task<ActionResult> GetUsers(Guid managerId, [FromServices] IUserTaskHandle handle)
        {
            try
            {
                return Ok(await handle.GetUsers(managerId));
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

        [HttpGet("managers")]
        [Authorize]
        public async Task<ActionResult> GetManagers([FromServices]IUserRepository repository)
        {
            try
            {
                var result = await repository.GetManagers();
                result.ToArray();

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

        [HttpPost("create")]
        [Authorize(Roles = "MANAGER")]
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
        [Authorize(Roles = "MANAGER")]
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
                    result = await handle.ImageIngestion(file, userId);
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

        [HttpPost("deletephoto")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult> DeletePhoto([FromBody] DeleteCommand command,
            [FromServices] IImageHandle handle)
        {
            try
            {
                if (!command.IsValid())
                    return BadRequest(command.Errors());

                var result = await handle.DeleteImage(command);

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

        [HttpPut("edit")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult> Edit([FromBody]UserEditCommand command,
            [FromServices]IEditUserCommandHandle handle)
        {
            try
            {
                if (!command.IsValid())
                    return BadRequest(command.Errors());

                var result = await handle.Edit(command);

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

        [HttpPost("delete")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult> Delete([FromBody]DeleteUserCommand command, 
            [FromServices]IDeleteUserCommandHandle handle)
        {
            try
            {
                if (!command.IsValid())
                    return BadRequest(command.Errors());

                var result = await handle.Delete(command);

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

        [HttpPost("removephone")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult> RemovePhone([FromBody]RemovePhoneCommand command,
            [FromServices] IRemovePhoneCommandHandle handle)
        {
            try
            {
                if (!command.IsValid())
                    return BadRequest(command.Errors());

                var result = await handle.Remove(command);

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

        [HttpPost("addphone")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult> AddPhone([FromBody]AddPhoneCommand command,
            [FromServices] IAddPhoneCommandHandle handle)
        {
            try
            {
                if (!command.IsValid())
                    return BadRequest(command.Errors());

                var result = await handle.Add(command);

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

    }
}
