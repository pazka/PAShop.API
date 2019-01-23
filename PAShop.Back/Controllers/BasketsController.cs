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
        private readonly IGenericService<Item> _itemService;
        private readonly IUserService _userService;

        public BasketsController(IGenericService<Basket> service, IHttpContextAccessor httpContextAccessor, IUserService userService, IGenericService<Item> itemService) : base(service,httpContextAccessor)
        {
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
            User user = _userService.Me(_httpContextAccessor.HttpContext.User);
            try
            {
                return Ok(user.Baskets);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Ok(_service.Add(new Basket(){State = State.NotValidated,Owner = user}));
            }

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

            Basket basketuser = user.Baskets.SingleOrDefault(b => b.State == State.NotValidated);
            Basket basket = _service.Get(basketuser.Id);
            
            basket.BasketItems.Add(new BasketItem()
            {
                Basket = basket,
                BasketId = basket.Id,
                Item = item,
                ItemId = item.Id
            });

            _service.Put(basket);

            return Ok(user.Baskets.SingleOrDefault(b => b.State == State.NotValidated));
        }

        [HttpPost("remove/{itemId}")]
        [Authorize(Roles = "LoggedUser")]
        public IActionResult RemoveItem(Guid itemId) {
            User user = _userService.Me(_httpContextAccessor.HttpContext.User);

            if (!ModelState.IsValid) {
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

        //    user.Baskets.SingleOrDefault(b => b.State == State.NotValidated)?.BasketItems.Remove();

            return Ok(user.Baskets.SingleOrDefault(b => b.State == State.NotValidated));
        }

        private bool BasketExists(Guid id)
        {
            return _service.Get(e => e.Id == id).Count == 1;
        }
    }
}