using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        protected IGenericRepository<T> Repo;

        public GenericService(IGenericRepository<T> repo)
        {
            this.Repo = repo;
        }

        public List<T> GetAll()
        {
            return Repo.GetAll();
        }

        public T Add(T obj)
        {
            return Repo.Add(obj);
        }

        public T GetOne(Expression<Func<T, Boolean>> predicate)
        {
            return Repo.GetOne(predicate);
        }
    }
}
