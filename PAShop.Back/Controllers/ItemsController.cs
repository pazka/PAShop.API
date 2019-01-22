using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Services.Interfaces;

namespace PAShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : GenericController<Item>
    {
        private IGenericService<StockMovement> _serviceStockMovement;
        public ItemsController(IGenericService<Item> service, IGenericService<StockMovement> serviceStockMovement) : base(service)
        {
            _serviceStockMovement = serviceStockMovement;
        }


        [HttpPost("{id}")]
        public IActionResult ChangeQuantity([FromRoute] Guid id, StockMovement stockMovement) {
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
    }
}