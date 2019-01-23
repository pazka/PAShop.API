using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Model.Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Services
{
    public class BasketService : GenericService<Basket>, IBasketService
    {
        private IGenericRepository<User> _userRepository;
        private IGenericRepository<Item> _itemRepository;

        public BasketService(IGenericRepository<Basket> repo,IGenericRepository<User> userRepository, IGenericRepository<Item> itemRepository) : base(repo)
        {
            _itemRepository = itemRepository;
            _userRepository = userRepository;
        }

        public Basket Mine(ClaimsPrincipal claimsPrincipal) {
            var email = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

            User user = _userRepository.Get(u => u.Email == email.Value).SingleOrDefault();
            Basket basket = _repository.Get(b => b.Owner.Id == user.Id && b.State == State.NotValidated)
                .SingleOrDefault();

            foreach (BasketItem basketItem in basket.BasketItems) {
                basketItem.Item = _itemRepository.Get(i => i.Id == basketItem.ItemId).SingleOrDefault();
            }

            return basket;
        }

        public double GetTotalPrice(Basket basket)
        {
            double res = 0;
            foreach (BasketItem basketItem in basket.BasketItems)
            {
                res += basketItem.Item.Price_HT * basketItem.Quantity + basketItem.Item.ShippingPrice;
            }

            return res;
        }
    }
}
