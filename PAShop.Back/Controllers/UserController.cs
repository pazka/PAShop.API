using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Services.Interfaces;
using Services.Services;

namespace PAShop.API.Controllers
{

    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "LoggedUser")]
        public User Get(Guid id)
        {
            return _userService.Get(u => u.Id == id).Single();
        }

        // POST api/<controller>/login
        [HttpPost("login")]
        public void Post([FromBody]string login, [FromBody]string password)
        {

        }

        // POST api/<controller>/new
        [HttpPost("new")]
        public User Post([FromBody]User user)
        {
            return _userService.Add(user);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "LoggedUser")]
        public void Put(int id, [FromBody]User user)
        {
            _userService.Put(user);
        }

        // DELETE api/<controller>/5
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "LoggedUser")]
        public void Delete(Guid id)
        {
           return  _userService.Delete(id);
        }
    }
}
