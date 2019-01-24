using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Model.Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Services
{
    public class GenericService<T> : IGenericService<T> where T : class,IGenericModel
    {
        protected IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repo)
        {
            this._repository = repo;
        }

        public List<T> GetAll() {
            return _repository.GetAll();
        }

        public List<T> GetAll(Expression<Func<T, Boolean>> predicate) {
            return _repository.Get(predicate);
        }

        public T Get(Guid id)
        {
            return _repository.Get(x => x.Id == id).SingleOrDefault();
        }

        public T Add(T obj)
        {
            return _repository.Add(obj);
        }

        public List<T> Get(Expression<Func<T, Boolean>> predicate)
        {
            return _repository.Get(predicate);
        }
        

        public T Put(T obj)
        {
            return _repository.Put(obj);
        }

        public T Delete(Guid id)
        {
            return _repository.Delete(_repository.Get(x => x.Id == id).SingleOrDefault());
        }

        public bool Exists(Guid id)
        {
            return _repository.Get(x => x.Id == id).Count == 1;
        }
    }
}
