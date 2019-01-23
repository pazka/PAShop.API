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

        public BasketService(IGenericRepository<Basket> repo,IGenericRepository<User> userRepository) : base(repo)
        {
            _userRepository = userRepository;
        }

        public Basket Mine(ClaimsPrincipal claimsPrincipal) {
            var email = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

            User user = _userRepository.Get(u => u.Email == email.Value).SingleOrDefault();

            return _repository.Get(b => b.Owner.Id == user.Id && b.State == State.NotValidated )
                .SingleOrDefault();
        }
    }
}
