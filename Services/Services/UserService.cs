using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private new IUserRepository _repository;

        public UserService(IUserRepository repo) : base(repo)
        {
            this._repository = repo;
            this.Sha1 = new SHA1CryptoServiceProvider();
        }

        internal String hash(String s)
        {
            return BitConverter.ToString(Sha1.ComputeHash(Encoding.UTF8.GetBytes(s)));
        }

        public User Add(User user)
        {   
            user.Password = this.hash(user.Password);
            Sha1.Clear();

            return this._repository.Add(user);
        }

        public User Authenticate(string login, string password)
        {
            User user = _repository
                .Get(u => (u.Email == login || u.Login == login) && (u.Password.Equals(this.hash(password)))).Single();

            if (user == null)
            {
                return null;
            }

            //authenticate

            return user;
        }

        public User Delete(Guid id)
        {
            return _repository.Delete(id);
        }
    }
}
