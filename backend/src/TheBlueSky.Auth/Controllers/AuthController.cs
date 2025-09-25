using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TheBlueSky.Auth.DTOs.Requests;
using TheBlueSky.Auth.DTOs.Responses;
using TheBlueSky.Auth.Models;
using TheBlueSky.Auth.Services;

namespace TheBlueSky.Auth.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IAuthTokenService _authTokenService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            IAuthTokenService authTokenService)
        {
            _userManager = userManager;
            _userService = userService;
            _authTokenService = authTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
        {
            try
            {
                var result = await _userService.RegisterUser(request);

                if (!result.Succeeded)
                    return BadRequest(new AuthResponse { Status = "Error", Message = "Registration failed. Please try again." });

                var createdUser = await _userManager.FindByEmailAsync(request.Email);

                if (createdUser == null)
                    return StatusCode(500, new AuthResponse { Status = "Error", Message = "Something went wrong. Please try again." });

                if (!await _userManager.IsInRoleAsync(createdUser, UserRoles.User))
                    await _userManager.AddToRoleAsync(createdUser, UserRoles.User);

                return Ok(new AuthResponse { Status = "Success", Message = "User created successfully!" });
            }
            catch
            {
                return StatusCode(500, new AuthResponse { Status = "Error", Message = "An error occurred. Please try again." });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new LoginResponse { Status = "Error", Message = "Invalid request." });

            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    return Unauthorized(new LoginResponse { Status = "Error", Message = "Invalid credentials." });

                var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!isValidPassword)
                    return Unauthorized(new LoginResponse { Status = "Error", Message = "Invalid credentials." });

                var token = await _authTokenService.CreateJwtToken(user);

                return Ok(new LoginResponse
                {
                    Status = "Success",
                    Message = "Login successful.",
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                });
            }
            catch
            {
                return StatusCode(500, new AuthResponse { Status = "Error", Message = "An error occurred. Please try again." });
            }
        }

        [HttpPost("admin/register")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> RegisterAdmin([FromBody] CreateUserRequest request)
        {
            try
            {
                var result = await _userService.RegisterUser(request);

                if (!result.Succeeded)
                    return BadRequest(new AuthResponse { Status = "Error", Message = "Registration failed. Please try again." });

                var createdUser = await _userManager.FindByEmailAsync(request.Email);

                if (createdUser == null)
                    return StatusCode(500, new AuthResponse { Status = "Error", Message = "Something went wrong. Please try again." });

                await _userManager.AddToRoleAsync(createdUser, UserRoles.Admin);

                return Ok(new AuthResponse { Status = "Success", Message = "Admin created successfully!" });
            }
            catch
            {
                return StatusCode(500, new AuthResponse { Status = "Error", Message = "An error occurred. Please try again." });
            }
        }
    }
}