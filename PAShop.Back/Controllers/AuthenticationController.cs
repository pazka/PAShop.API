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

        public AuthenticationController(IConfiguration config, IHttpContextAccessor httpContextAccessor,IUserService userService) : base()
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _authenticationHelper = new AuthenticationHelper(_config,userService);
        }

        [AllowAnonymous, HttpPost("login")]
        public IActionResult CreateToken([FromBody] dynamic login)
        {
            if (login["Email"] == null || login["Password"] == null)
            {
                return BadRequest("Missing Email or Password");
            }
            var user = _authenticationHelper.Authenticate(login);

            if (user == null) return StatusCode(403,"Bad Credentials");

            var tokenString = _authenticationHelper.BuildToken(user);

            return Ok(new { user = user, token = tokenString });

        }

        [HttpGet("me")]
        [Authorize("User")]
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