using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using MoreLinq;
using Services.Interfaces;

namespace PAShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController, EnableCors("CORS")]
    public class ItemsController : GenericController<Item>
    {
        private IGenericService<StockMovement> _stockMovementService;
        private IGenericService<Inventory> _inventoryService;

        public ItemsController(IGenericService<Item> service,IHttpContextAccessor httpContextAccessor, IGenericService<StockMovement> stockMovementService, IGenericService<Inventory> inventoryService) : base(service,httpContextAccessor)
        {
            _inventoryService = inventoryService;
            _stockMovementService = stockMovementService;
        }

        [AllowAnonymous, HttpGet("tva")]
        public Dictionary<string,TvaType> GetTva() {

            Dictionary<string,TvaType> dic = new Dictionary<string, TvaType>();
            foreach (TvaType curr_type in Enum.GetValues(typeof(TvaType)).Cast<TvaType>())
            {
                dic.Add(Enum.GetName(typeof(TvaType), curr_type),curr_type);
            }

            return dic;
        }

        [HttpPost("{itemId}/change")]
        [Authorize("Vendor")]
        public IActionResult ChangeQuantityDisp([FromRoute] Guid itemId,[FromBody] int amount) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            Item item;

            try
            {
                item = _service.Get(i => i.Id == itemId).SingleOrDefault();
            }
            catch (DbUpdateException)
            {
                if (_service.Exists(itemId))
                {
                    return BadRequest("Item not found.");
                }
                else
                {
                    throw;
                }
            }

            StockMovement stockMovement = new StockMovement()
            {
                Amount = amount,
                LastInventory = _inventoryService.Get(i => i.Item.Id == itemId).MaxBy(i => i.Timestamp).SingleOrDefault(),
                Item = item,
                Timestamp = DateTime.Now
            };

            _stockMovementService.Add(stockMovement);

            return null;//TODO
        }

        [HttpPost]
        [Authorize("Vendor")]
        public override IActionResult New(Item item)
        {
            item = _service.Add(item);

            Inventory inventory = _inventoryService.Add(new Inventory()
            {
                Item = item,
                Quantity = 0,
                Timestamp = DateTime.Now
            });

            _stockMovementService.Add(new StockMovement()
            {
                Amount = 0,
                Item = item,
                LastInventory = inventory,
                Timestamp = DateTime.Now
            });

            return Ok(_service.Get(i=> i.Id == item.Id));
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public IActionResult Search([FromQuery] string tags)
        {
            string[] tags_arr = tags.Split();

            //laids mais pas le choix 
            List<Item> allItems = _service.GetAll();

            return Ok(allItems.Where(i => i.Label.Split().Intersect(tags_arr).Any()));
        }
    }
}