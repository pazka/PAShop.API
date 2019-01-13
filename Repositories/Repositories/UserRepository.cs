using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class UserRepository :  GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContext dbContextType) : base(dbContextType)
        {
        }

        public User Delete(Guid id)
        {
            var dbSet = context.Set<User>();
            var query = dbSet.Where(u => u.Id == id);
            User user = query.SingleOrDefault();

            if (user == null)
            {
                return null;
            }

            context.Attach(user);
            user.Deleted = true;
            context.Set<User>().Update(user);
            context.SaveChanges();

            return user;
        }
    }
}
