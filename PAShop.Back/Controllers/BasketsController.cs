using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Migrations;
using Model.Models;
using MoreLinq;
using Repositories.Interfaces;
using Services.Interfaces;

namespace PAShop.API.Controllers
{
    //TODO TEst sur basket car classe contenant le plus de logique
    //TODO réorganiser le code car trop de logique métier dans le controlleur
    [Route("api/[controller]")]
    [ApiController, EnableCors("CORS")]
    public class BasketsController : GenericController<Basket>
    {
        private new readonly IBasketService _service;
        private readonly IGenericService<StockMovement> _stockMovementService;
        private readonly IGenericService<Inventory> _inventoryService;
        private readonly IItemService _itemService;
        private readonly IUserService _userService;
        private readonly IGenericService<Transaction> _transactionService;

        public BasketsController(IBasketService service, IHttpContextAccessor httpContextAccessor, IUserService userService, IItemService itemService,IGenericService<Transaction> transactionService, IGenericService<StockMovement> stockMovementService, IGenericService<Inventory> inventoryService) : base(service,httpContextAccessor) {
            _stockMovementService = stockMovementService;
            _inventoryService = inventoryService;
            _transactionService = transactionService;
            _service = service;
            _userService = userService;
            _itemService = itemService;
        }

        // GET: api/Baskets
        [HttpGet]
        [Authorize("Admin")]
        public IEnumerable<Basket> GetBaskets()
        {
            return _service.GetAll();
        }

        [HttpGet("mine")]
        [Authorize("User")]
        public IActionResult GetMyBasket()
        {
            Basket basket;
            try {
                basket = _service.Mine(_httpContextAccessor.HttpContext.User);
            }
            catch (Exception e)
            {
                return StatusCode(500,e.ToString());
            }

            return Ok(basket);

        }

        [HttpPost("add/{itemId}")]
        [Authorize("User")]
        public IActionResult AddItem(Guid itemId)
        {
            User user = _userService.Me(_httpContextAccessor.HttpContext.User);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Item item;

            try {
               item = _itemService.Get(itemId);
               Dictionary<StockMovementType, int> stocks = _itemService.GetGlobalQuantity(itemId);
               if (stocks.GetValueOrDefault(StockMovementType.Regular) <= 0)
               {
                   return Ok($"No stock left ( {stocks.GetValueOrDefault(StockMovementType.Reserved)} reserved).");
               }
            }
            catch (DbUpdateException e) {
                if (_itemService.Exists(itemId)) {
                    return Ok("Item not found");
                }
                else {
                    throw;
                }
            }

            Basket basket = _service.Mine(_httpContextAccessor.HttpContext.User);

            try
            {
                basket.BasketItems.SingleOrDefault(bi => bi.ItemId == itemId).Quantity++;
            }
            catch (Exception)
            {
                basket.BasketItems.Add( new BasketItem()
                {
                    BasketId = basket.Id,
                    ItemId = item.Id,
                    Quantity = 1
                });
            }

            _stockMovementService.Add(new StockMovement()
            {
                Amount = 1,
                LastInventory = _inventoryService.Get(i => i.Item.Id == itemId).MaxBy(i => i.Timestamp).SingleOrDefault(),
                Item = item,
                Timestamp = DateTime.Now,
                Type = StockMovementType.Reserved
            });

            _stockMovementService.Add(new StockMovement()
            {
                Amount = -1,
                LastInventory = _inventoryService.Get(i => i.Item.Id == itemId).MaxBy(i => i.Timestamp).SingleOrDefault(),
                Item = item,
                Timestamp = DateTime.Now,
                Type = StockMovementType.Regular
            });

            _service.Put(basket);

            return Ok(_service.Mine(_httpContextAccessor.HttpContext.User));
        }

        [HttpPost("remove/{itemId}")]
        [Authorize("User")]
        public IActionResult RemoveItem(Guid itemId) {
            User user = _userService.Me(_httpContextAccessor.HttpContext.User);

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            Basket basket = _service.Mine(_httpContextAccessor.HttpContext.User);
            BasketItem basketItem;

            try
            {
                basketItem = basket.BasketItems.SingleOrDefault(bi => bi.ItemId == itemId);
            }
            catch (Exception)
            {
                return BadRequest("Item not in basket.");
            }

            if (basketItem == null) {
                return Ok(_service.Mine(_httpContextAccessor.HttpContext.User)); 
            }

            basket.BasketItems.Remove(basketItem);
            basketItem.Quantity--;

            _stockMovementService.Add(new StockMovement()
            {
                Amount = -1,
                LastInventory = _inventoryService.Get(i => i.Item.Id == itemId).MaxBy(i => i.Timestamp).SingleOrDefault(),
                Item = basketItem.Item,
                Timestamp = DateTime.Now,
                Type = StockMovementType.Reserved
            });

            _stockMovementService.Add(new StockMovement()
            {
                Amount = 1,
                LastInventory = _inventoryService.Get(i => i.Item.Id == itemId).MaxBy(i => i.Timestamp).SingleOrDefault(),
                Item = basketItem.Item,
                Timestamp = DateTime.Now,
                Type = StockMovementType.Regular
            });

            if (basketItem.Quantity > 0) 
                basket.BasketItems.Add(basketItem);
                
            _service.Put(basket);

            return Ok(_service.Mine(_httpContextAccessor.HttpContext.User));
        }

        [HttpPost("validate/{basketId}")]
        [Authorize("User")]
        public IActionResult ValidateBasket(Guid basketId)
        {

            User user = _userService.Me(_httpContextAccessor.HttpContext.User);
            Basket basket = _service.Get(b => b.Id == basketId && b.State == BasketState.NotValidated).SingleOrDefault();

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (basket == null)
            {
                return BadRequest("Basket not found");
            }
            else if (basket.Owner.Id != user.Id && user.Role < Role.Admin)
            {
                return StatusCode(403,"Not your basket and not an Admin.");
            }

            Transaction transaction = new Transaction()
            {
                Order = basket,
                OrderId = basketId,
                Owner = user,
                OwnerId = user.Id,
                State = TransactionState.Payed
            };

            foreach (BasketItem bi in basket.BasketItems)
            {
                _stockMovementService.Add(new StockMovement()
                {
                    Amount = - bi.Quantity,
                    LastInventory = _inventoryService.Get(i => i.Item.Id == bi.ItemId).MaxBy(i => i.Timestamp).SingleOrDefault(),
                    
                    Item = bi.Item,
                    Timestamp = DateTime.Now,
                    Type = StockMovementType.Reserved
                });
            }

            basket.Transaction = transaction;
            basket.State = BasketState.Payed;

            _transactionService.Add(transaction);
            _service.Put(basket);

            return Ok(_service.Mine(HttpContext.User));
        }
    }
}