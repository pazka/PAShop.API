using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Model.Models;

namespace Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> GetAll();
        T Add(T obj);
        List<T> Get(Expression<Func<T, Boolean>> predicate);
        T Put(T obj);
        T Delete(T obj);
    }
}
