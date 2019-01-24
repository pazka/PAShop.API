using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Model.Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        private SHA1CryptoServiceProvider Sha1;
        private new IGenericRepository<User> _repository;
        private IGenericRepository<Basket> _basketRepository;

        public UserService(IGenericRepository<User> repo, IGenericRepository<Basket> basketRepository) : base(repo)
        {
            this._repository = repo;
            _basketRepository = basketRepository;
            this.Sha1 = new SHA1CryptoServiceProvider();
        }

        internal string Hash(string text)
        {

            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        public new User Add(User user)
        {   
            user.Password = this.Hash(user.Password);
            Sha1.Clear();

            if (_repository.Get(u => u.Email == user.Email).Count > 0)
            {
                return null;
            }

            user.Deleted = false;
            user = this._repository.Add(user);
            if (user != null)
            {
                Basket basket = new Basket()
                {
                    Owner = user,
                    State = BasketState.NotValidated,
                    BasketItems = new List<BasketItem>()
                };
                _basketRepository.Add(basket);
            }

            return user;
        }

        public User Authenticate(string login, string password)
        {
            User user = _repository
                .Get(u =>  u.Email == login  && u.Password.Equals(this.Hash(password))).FirstOrDefault();

            //authenticate

            return user;
        }

        public User Delete(Guid id)
        {
            User user = _repository.Get(u => u.Id == id).SingleOrDefault();

            if (user == null)
            {
                return null;
            }

            user.Deleted = true;

            return _repository.Put(user);
        }

        public User Me(ClaimsPrincipal claimsPrincipal) { 
            var email = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

            return this.Get(u => u.Email == email.Value).SingleOrDefault();
        }
    }
}
