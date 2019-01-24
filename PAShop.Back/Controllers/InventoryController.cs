using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Services.Interfaces;

namespace PAShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController, EnableCors("CORS")]
    public class InventoryController : GenericController<Inventory>
    {
        private IItemService _itemService;

        public InventoryController(IGenericService<Inventory> service, IHttpContextAccessor httpContextAccessor,IItemService itemService) : base(service, httpContextAccessor)
        {
            _itemService = itemService;
        }

        [HttpPost("item/{itemId}")]
        [Authorize("Vendor")]
        public IActionResult New(Guid itemId)
        {
            Dictionary<StockMovementType,int> globalStock = _itemService.GetGlobalQuantity(itemId);


            Inventory inventory = new Inventory()
            {
                Item = _itemService.Get(i=>i.Id== itemId).SingleOrDefault(),
                //Arbitraire
                Quantity = globalStock.GetValueOrDefault(StockMovementType.Regular) - globalStock.GetValueOrDefault(StockMovementType.Stolen),
                Timestamp = DateTime.Now
            };

            _service.Add(inventory);
            return Ok(inventory);
        }



        [HttpGet("item/{itemId}")]
        [Authorize("Vendor")]
        public IActionResult GetStock(Guid itemId) {
            return Ok(_itemService.GetGlobalQuantity(itemId));
        }
    }
}
