using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Model.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using PAShop.API.Helper;
using Services.Interfaces;
using Services.Services;
using Microsoft.AspNetCore.Cors;

namespace PAShop.API.Controllers
{
    [Route("api/auth")]
    [ApiController, EnableCors("CORS")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationHelper _authenticationHelper;

        public AuthenticationController(IConfiguration config, IHttpContextAccessor httpContextAccessor,IUserService userService)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _authenticationHelper = new AuthenticationHelper(_config,userService);
        }

        [AllowAnonymous, HttpPost("login")]
        public IActionResult CreateToken([FromBody] dynamic login)
        {
            IActionResult response = Unauthorized();

            var user = _authenticationHelper.Authenticate(login);

            if (user == null) return response;

            var tokenString = _authenticationHelper.BuildToken(user);
            response = Ok(new { token = tokenString });
            return response;

        }

        [HttpGet("me")]
        [Authorize(Roles = "LoggedUser")]
        public IActionResult MySelf()
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var email = currentUser.FindFirst(ClaimTypes.NameIdentifier);

            if (currentUser == null || email == null)
            {
                return BadRequest("I don't know you");
            }

            return Ok(_authenticationHelper.GetUserService().Get(u => u.Email == email.Value));
        }
    }
}