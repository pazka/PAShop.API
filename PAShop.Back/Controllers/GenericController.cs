using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace PAShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController : ControllerBase
    {
        private readonly IGenericService<IGenericModel> _service;

        public GenericController(IGenericService<IGenericModel> service) {
            _service = service;
        }

        // GET: api/Baskets
        [HttpGet]
        public IEnumerable<IGenericModel> Get() {
            return _service.GetAll();
        }

        // GET: api/Baskets/5
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] Guid id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var obj = _service.Get(b => b.Id == id).SingleOrDefault();

            if (obj == null) {
                return NotFound();
            }

            return Ok(obj);
        }

        // PUT: api/Baskets/5
        [HttpPut("{id}")]
        public IActionResult PutBasket([FromRoute] Guid id, [FromBody] dynamic obj) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != obj.Id) {
                return BadRequest();
            }

            try {
                return Ok(_service.Put(obj));
            }
            catch (DbUpdateConcurrencyException) {
                if (!_service.Exists(id)) {
                    return NotFound();
                }
                throw;
            }
        }

        // POST: api/Baskets
        [HttpPost]
        public IActionResult PostBasket([FromBody] dynamic obj) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            try {
                obj = _service.Add(obj);
            }
            catch (DbUpdateException) {
                if (_service.Exists(obj.Id)) {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else {
                    throw;
                }
            }

            return CreatedAtAction("Get", new { id = obj.Id }, obj);
        }
        
        private bool Exists(Guid id) {
            return _service.Exists(id);
        }
    }
}