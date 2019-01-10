using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Model.Models;
using NFE104._1.Helper;
using System.Security.Claims;

namespace NFE104._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationHelper _authenticationHelper;

        public AuthController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _authenticationHelper = new AuthenticationHelper(_config);
        }

        [AllowAnonymous, HttpPost("login")]
        public IActionResult CreateToken([FromBody] User login)
        {
            IActionResult response = Unauthorized();
            var user = _authenticationHelper.Authenticate(login);

            if (user == null) return response;

            var tokenString = _authenticationHelper.BuildToken(user);
            response = Ok(new {token = tokenString});
            return response;

        }

        [HttpGet("me")]
        public IActionResult MySelf()
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var email = currentUser.FindFirst(ClaimTypes.NameIdentifier);

            if (currentUser == null || email == null)
            {
                return BadRequest("I don't know you");
            }

            return Ok(email.Value);
        }

    }
}