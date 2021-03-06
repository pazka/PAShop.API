﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using EFCustomAnnotations;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Models;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class,IGenericModel
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

        public List<T> Get(Expression<Func<T, Boolean>> predicate)
        {
            var dbSet = context.Set<T>();
            var query = dbSet.Where(predicate).AsQueryable();

            foreach (PropertyInfo prop in typeof(T).GetProperties()) {
                if (Attribute.IsDefined(prop, typeof(IncludeProperty)))
                {
                    query = query.Include(prop.Name);
                }
            }

            return query.ToList();
        }

        public T Put(T obj)
        {
            context.Attach(obj);
            context.Entry(obj).State = EntityState.Modified;
            context.Set<T>().Update(obj);
            context.SaveChanges();
            return obj;
        }

        public T Delete(T obj)
        {
            context.Attach(obj);
            context.Set<T>().Remove(obj);
            context.SaveChanges();

            return context.Set<T>().Find(obj);
        }
    }
}
