﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        protected IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repo)
        {
            this._repository = repo;
        }

        public List<T> GetAll()
        {
            return _repository.GetAll();
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

        public T Delete(T obj)
        {

            return _repository.Delete(obj);
        }
    }
}