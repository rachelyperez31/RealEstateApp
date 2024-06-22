using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.DTOs.Account;
using RealEstateApp.Core.Application.DTOs.Agent;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Services.Account;
using RealEstateApp.WebApi.Middlewares;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Account Management")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Login")]
        [SwaggerOperation(
         Summary = "Login of users",
         Description = "Authenticate an user in the system and returns a JWT"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest authenticationRequest)
        {
            return Ok(await _accountService.AuthenticateWebApiAsync(authenticationRequest));
        }

        [Authorize(Roles = "Admin,Developer")]
        [HttpPost("Register of Developers")]
        [SwaggerOperation(
            Summary = "Register a developer",
            Description = "Gets all required parameters to create a developer"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegisterDeveloperAsync(RegisterRequest request)
        {
            //request.Role = Roles.Developer.ToString();
            return Ok(await _accountService.RegisterUserAsync(request, role: Roles.Developer.ToString()));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Register of Administrators")]
        [SwaggerOperation(
            Summary = "Register an administrator",
            Description = "Gets all required parameters to create an administrator"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegisterAdminAsync(RegisterRequest request)
        {
            //request.Role = Roles.Admin.ToString();
            return Ok(await _accountService.RegisterUserAsync(request, role: Roles.Admin.ToString()));
        }
    }
}
