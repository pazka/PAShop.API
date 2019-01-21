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
    public class BasketsController : GenericController<Basket>
    {
        private readonly IGenericService<Basket> _service;
        private readonly IGenericService<Item> _itemService;

        public BasketsController(IGenericService<Basket> service) : base(service)
        {
            _service = service;
        }

        // GET: api/Baskets
        [HttpGet]
        public IEnumerable<Basket> GetBaskets()
        {
            return _service.GetAll();
        }

        // GET: api/Baskets/5
        [HttpGet("{id}")]
        public IActionResult GetBasket([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var basket = _service.Get(b => b.Id == id).SingleOrDefault();

            if (basket == null)
            {
                return NotFound();
            }

            return Ok(basket);
        }

        // PUT: api/Baskets/5
        [HttpPut("{id}")]
        public IActionResult PutBasket([FromRoute] Guid id, [FromBody] Basket basket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != basket.Id)
            {
                return BadRequest();
            }

            try
            {
                return Ok(_service.Put(basket));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BasketExists(id))
                {
                    return NotFound();
                }
                    throw;
            }
        }

        [HttpPost("{id}/add")]
        public IActionResult AddItem([FromRoute] Guid id, Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Basket basket;

            try
            {
                basket = _service.Get(b => b.Id == id).SingleOrDefault();
            }
            catch (DbUpdateException)
            {
                if (BasketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            basket.Items.Add(item);

            return CreatedAtAction("GetBasket", new { id = basket.Id }, basket);
        }

        [HttpPost("{id}/remove")]
        public IActionResult RemoveItem([FromRoute] Guid basketId, Guid itemId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Basket basket;

            try
            {
                basket = _service.Get(b => b.Id == basketId).SingleOrDefault();
            }
            catch (DbUpdateException)
            {
                if (BasketExists(basketId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Item item;

            try
            {
                item = _itemService.Get(i => i.Id == basketId).SingleOrDefault();
            }
            catch (DbUpdateException)
            {
                if (_itemService.Exists(itemId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            basket.Items.Remove(item);

            return CreatedAtAction("GetBasket", new { id = basket.Id }, basket);
        }

        private bool BasketExists(Guid id)
        {
            return _service.Get(e => e.Id == id).Count == 1;
        }
    }
}