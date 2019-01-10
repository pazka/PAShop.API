using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Model.Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Services
{
    public class UserService : GenericService<User>
    {
        private SHA1CryptoServiceProvider Sha1;

        public UserService(IGenericRepository<User> repo, SHA1CryptoServiceProvider sha1) : base(repo)
        {
            this.Sha1 = sha1;
        }

        public new User Add(User user)
        {   
            user.Password = Encoding.Default.GetString(Sha1.ComputeHash(Encoding.ASCII.GetBytes(user.Password)));
            Sha1.Clear();

            return this._repository.Add(user);
        }
    }
}
