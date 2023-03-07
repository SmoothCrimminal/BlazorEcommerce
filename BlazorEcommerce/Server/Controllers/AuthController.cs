using BlazorEcommerce.Server.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register( [FromBody] UserRegister request)
        {
            var result = await _authService.Register(new Shared.User() { Email = request.Email }, request.Password);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login([FromBody] UserLogin request)
        {
            var result = await _authService.Login(request.Email, request.Password);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] string newPassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _authService.ChangePassword(int.Parse(userId), newPassword);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
