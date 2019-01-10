using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Services.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        List<T> GetAll();
        T Add(T obj);
        T GetOne(Expression<Func<T, Boolean>> predicate);
    }
}
