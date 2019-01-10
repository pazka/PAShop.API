using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Models;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbContext context;

        public GenericRepository(DbContext dbContextType)
        {
            context = dbContextType;
        }

        public List<T> GetAll()
        {
            var dbSet = context.Set<T>();
            var query = dbSet.AsQueryable();
            return query.ToList();
        }

        public T Add(T obj)
        {
            context.Attach(obj);
            context.Set<T>().Add(obj);

            context.SaveChanges();
            return obj;
        }

        public T GetOne(Expression<Func<T, Boolean>> predicate)
        {
            var dbSet = context.Set<T>();
            var query = dbSet.Where(predicate);
            return query.Single();
        }
    }
}
