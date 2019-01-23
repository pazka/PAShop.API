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
using Model.Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace PAShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController, EnableCors("CORS")]
    public class BasketsController : GenericController<Basket>
    {
        private new readonly IBasketService _service;
        private readonly IGenericService<Item> _itemService;
        private readonly IUserService _userService;

        public BasketsController(IBasketService service, IHttpContextAccessor httpContextAccessor, IUserService userService, IGenericService<Item> itemService) : base(service,httpContextAccessor)
        {
            _service = service;
            _userService = userService;
            _itemService = itemService;
        }

        // GET: api/Baskets
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IEnumerable<Basket> GetBaskets()
        {
            return _service.GetAll();
        }

        [HttpGet("mine")]
        [Authorize(Roles = "LoggedUser")]
        public IActionResult GetMyBasket()
        {
            Basket basket;
            try {
                basket = _service.Mine(_httpContextAccessor.HttpContext.User);
            }
            catch (Exception e)
            {
                return NotFound();
            }

            return Ok(basket);

        }

        [HttpPost("add/{itemId}")]
        [Authorize(Roles = "LoggedUser")]
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
            }
            catch (DbUpdateException) {
                if (_itemService.Exists(itemId)) {
                    return NotFound();
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

            _service.Put(basket);

            return Ok(_service.Mine(_httpContextAccessor.HttpContext.User));
        }

        [HttpPost("remove/{itemId}")]
        [Authorize(Roles = "LoggedUser")]
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
                return NotFound("Item not in basket.");
            }

            if (basketItem == null) {
                return Ok(_service.Mine(_httpContextAccessor.HttpContext.User)); 
            }

            basket.BasketItems.Remove(basketItem);
                basketItem.Quantity--;

            if(basketItem.Quantity >  0)
                basket.BasketItems.Add(basketItem);
                
            _service.Put(basket);

            return Ok(_service.Mine(_httpContextAccessor.HttpContext.User));
        }
    }
}