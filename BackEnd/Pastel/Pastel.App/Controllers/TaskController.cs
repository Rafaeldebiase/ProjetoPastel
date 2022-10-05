using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pastel.Data.Interfaces;
using Pastel.Domain.Command;
using Pastel.Handles.Interfaces;

namespace Pastel.App.Controllers
{
    [Route("api/v1/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;

        public TaskController(ILogger<TaskController> logger)
        {
            _logger = logger;
        }

        [HttpGet("gettasks")]
        [Authorize]
        public async Task<ActionResult> GetTasks(Guid userId, [FromServices] ITaskRepository repository)
        {
            try
            {
                var result = await repository.GetTaskByUserId(userId);

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

        [HttpPost("close")]
        [Authorize]
        public async Task<ActionResult> Close([FromBody] CloseTaskCommand command,
            [FromServices] ICloseTaskCommandHandle handle)
        {
            try
            {
                if (!command.IsValid())
                    return BadRequest(command.Errors());

                var result = await handle.Close(command);

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

        [HttpPost("create")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult> Create([FromBody]CreateTaskCommand command,
            [FromServices]ICreateTaskCommandHandle handle)
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

        [HttpPut("edit")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult> Edit([FromBody]EditTaskCommand command,
            [FromServices] IEditTaskCommandHandle handle)
        {
            try
            {
                if (!command.IsValid())
                    return Ok(command.Errors());

                var result = await handle.Edit(command);

                if (result.Errors.Count > 0)
                {
                    return Ok(result);
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
        public async Task<ActionResult> Delete([FromBody]DeleteCommand command,
            [FromServices]IDeleteTaskCommandHandle handle)
        {
            try
            {
                if (!command.IsValid())
                    return BadRequest(command.Errors());

                var result = await handle.Delete(command);

                if (result.Errors.Count > 0)
                {
                    return Ok(result);
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
