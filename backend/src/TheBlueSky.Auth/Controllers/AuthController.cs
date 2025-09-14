using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlueSky.Auth.DTOs.Requests;
using TheBlueSky.Auth.DTOs.Responses;
using TheBlueSky.Auth.Services;

namespace TheBlueSky.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
        {
            var result = await _userService.RegisterUser(request);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Status = "Error",
                    result.Errors
                });
            }

            return Ok(new AuthResponse
            { 
                Status = "Success",
                Message = "User created successfully!" 
            });

        }


    }
}
