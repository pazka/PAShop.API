using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    class BasketRepository :  GenericRepository<Basket>
    {
        public BasketRepository(DbContext dbContextType) : base(dbContextType)
        {
        }
    }
}
