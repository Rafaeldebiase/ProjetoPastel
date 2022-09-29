using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pastel.App.Token;
using Pastel.Domain.Command;
using Pastel.Handles.Interfaces;

namespace Pastel.App.Controllers
{
    [Route("api/v1/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("autenticate")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody]AutenticateCommand command,
            [FromServices]IAutenticateCommandHandle handle, [FromServices]IConfiguration configuration)
        {
            if (!command.IsValid())
                return BadRequest(command.Errors());

            var resultUserDto = await handle.Autenticate(command);

            if (resultUserDto.Errors.Count > 0)
                return NotFound(resultUserDto);
            
            var secret = configuration.GetSection("secret").Value;
            var token = TokenService.GenerateToken(resultUserDto.User, secret);

            return Ok(
                    new
                    {
                        user = resultUserDto?.User?.FullName.FirstName,
                        token = token
                    }
                );
        }
    }
}
