using BookStore.Core.Repositories.Contract;
using BookStore.Dtos.Incoming;
using BookStore.Dtos.outgoingDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {

            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var user = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.Email
            };
            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            if (registerRequestDto.Roles != null && registerRequestDto.Roles.Length > 0)
            {
                var roleResult = await _userManager.AddToRolesAsync(user, registerRequestDto.Roles);
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }
            }
            return Ok("User registered successfully");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                return Unauthorized("Invalid username or password");
            }
            var role = await _userManager.GetRolesAsync(user);
            if(role != null)
            {
                var userRoles = role.ToList();
                var jwtToken =  _tokenRepository.CreateJwtToken(user , userRoles);
                var response = new LoginResponseDto
                {
                    Token = jwtToken,
                };
                return Ok(response);
            }
            return Ok("Login successful");
        }
    }
}
