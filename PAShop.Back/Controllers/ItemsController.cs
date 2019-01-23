using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Services.Interfaces;

namespace PAShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController, EnableCors("CORS")]
    public class ItemsController : GenericController<Item>
    {
        private IGenericService<StockMovement> _serviceStockMovement;
        public ItemsController(IGenericService<Item> service,IHttpContextAccessor httpContextAccessor, IGenericService<StockMovement> serviceStockMovement) : base(service,httpContextAccessor)
        {
            _serviceStockMovement = serviceStockMovement;
        }


        [HttpPost("{id}")]
        public IActionResult ChangeQuantityDisp([FromRoute] Guid id, StockMovement stockMovement) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            Item item;

            try
            {
                item = _service.Get(i => i.Id == stockMovement.Item.Id).SingleOrDefault();
            }
            catch (DbUpdateException)
            {
                if (_service.Exists(stockMovement.Item.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            stockMovement.Item = item;

            return Ok(_serviceStockMovement.Get(sm => sm.Item.Id == id));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string tags)
        {
            string[] tags_arr = tags.Split();

            //laids mais pas le choix 
            List < Item > allItems = _service.GetAll();

            return Ok(allItems.Where(i=> i.Label.Split().Intersect(tags_arr).Any()));
        }
    }
}