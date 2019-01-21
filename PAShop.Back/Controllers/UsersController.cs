using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Models;
using Newtonsoft.Json.Serialization;
using Services.Interfaces;
using Services.Services;

namespace PAShop.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserService _userService;
    
        public UsersController(IUserService us)
        {
            this._userService = (UserService)us;
        }

        // POST api/<controller>
        [HttpPost()]
        public ActionResult<User> New([FromBody]User user)
        {
            
            User newUser = _userService.Add(user);

            if (newUser == null)
            {
                return new BadRequestObjectResult($"User with email : {user.Email} already exist");
            }

            return Ok(user);
        }

        // GET api/<controller>/all
        [HttpGet("")]
        public ActionResult<IEnumerable<User>> All()
        {
            return _userService.GetAll();
        }


        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public ActionResult<User> Get(Guid id)
        {
            List<User> userList = _userService.Get(u => u.Id == id);

            if (!userList.Any())
                return NotFound("No user at this id");
            if (userList.Count > 1)
                return NotFound("Several users at this Id");

            return userList.Single();
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public ActionResult<User> Put([FromRoute]Guid id, [FromBody]User user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }


            return _userService.Put(user);
        }

        // DELETE api/<controller>/5
        [HttpDelete("")]
        [Authorize(Roles = "User")]
        public ActionResult DeleteSelf()
        {
            User user = _userService.Get(u => u.Email == HttpContext.User.FindFirst(ClaimTypes.Email).Value).SingleOrDefault();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (user == null)
                return NotFound();

            if (_userService.Delete(user.Id) == null)
                return StatusCode(500, "User could not be deleted");

            return Ok(User);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Guid id)
        {
            User user;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if ((user = _userService.Delete(id)) == null)
                return NotFound();

            return Ok(user);
        }
    }
}
