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
        private readonly IGenericService<StockMovement> _stockMovementService;
        private readonly IGenericService<Inventory> _inventoryService;
        private readonly IItemService _itemService;

        public ItemsController(IItemService service,IHttpContextAccessor httpContextAccessor, IGenericService<StockMovement> stockMovementService, IGenericService<Inventory> inventoryService) : base(service,httpContextAccessor)
        {
            _itemService = service;
            _inventoryService = inventoryService;
            _stockMovementService = stockMovementService;
        }

        [AllowAnonymous, HttpGet("tva")]
        public List<dynamic> GetTva() {

            List<dynamic> dic = new List<dynamic>();
            foreach (TvaType curr_type in Enum.GetValues(typeof(TvaType)).Cast<TvaType>())
            {
                dic.Add(new{name = Enum.GetName(typeof(TvaType), curr_type), value = curr_type });
            }

            return dic;
        }

        [AllowAnonymous, HttpGet("hot")]
        public ActionResult GetHotProduct() {
            return Ok(_itemService.GetAll().Where(i => i.Deleted == false).OrderBy(i => i.Priority).Take(3));
        }
        [AllowAnonymous, HttpGet("active")]
        public ActionResult GetActiveProduct() {
            return Ok(_itemService.GetAll().Where(i => i.Deleted == false));
        }

        [HttpPost("{itemId}/change")]
        [Authorize("Vendor")]
        public IActionResult ChangeQuantityDisp([FromRoute] Guid itemId,[FromBody] dynamic body) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (body.amount == null) {
                return BadRequest("body.amount not found");
            }
            if (body.type == null) {
                return BadRequest("body.type not found");
            }

            Item item;
            int amount = body.amount;
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
                Timestamp = DateTime.Now,
                Type = body.type
            };

            _stockMovementService.Add(stockMovement);

            return Ok(new{stock = _itemService.GetGlobalQuantity(itemId), item = _itemService.Get(i => i.Id == itemId)});
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