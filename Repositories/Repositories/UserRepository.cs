using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(DbContext dbContextType) : base(dbContextType)
        {
        }

        public new bool Delete(Guid id)
        {
            User user = this.Get(u => u.Id == id).Single();

            //TODO always null ?
            if (user.Deleted || user == null)
            {
                return false;
            }

            context.Attach(user);
            user.Deleted = true;
            context.Set<User>().Update(user);
            context.SaveChanges();

            return true;
        }
    }
}
