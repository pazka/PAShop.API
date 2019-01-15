using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    
        public UsersController(IGenericService<User> us)
        {
            this._userService = (UserService)us;
        }

        // POST api/<controller>
        [HttpPost("new")]
        public ActionResult<User> New([FromBody]User user)
        {
            return _userService.Add(user);
        }

        // GET api/<controller>/all
        [HttpGet("all")]
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

            if (userList.Any())
                return NotFound("Not user at this id");
            if (userList.Count > 1)
                return NotFound("Several users at this Id");

            return userList.Single();
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public ActionResult<User> Put([FromRoute]int id, [FromBody]User user)
        {
            return _userService.Put(user);
        }

        // DELETE api/<controller>/5
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Guid id)
        {
            if (_userService.Delete(id) == null)
                return StatusCode(500, "User not found");

            return StatusCode(200,User);
        }
    }
}
