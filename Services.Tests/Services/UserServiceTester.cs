﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Models;
using Moq;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services.Services;

namespace Services.Tests.Services
{
        [TestClass]
        public class UserServicesTester
    {
        private IUserRepository _repository;
        private UserService _service;

        public UserServicesTester()
        {
            _repository = new UserRepository(null);
            _service = new UserService(_repository);
        }

        [TestMethod]
        public void GivenUserExists_WhenDeleteUser_ThenReturnUserDeleted()
        {
            var mock = new Mock<IUserRepository>();
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                LastName = "Lasttest",
                Address = "2 rue du osef",
                Deleted = true
            };

            mock.Setup(p => p.Delete(user.Id))
                .Returns(user);

            var service = new UserService(mock.Object);
            var res = service.Delete(user.Id);
            Assert.IsTrue(res.Deleted);
        }

        [TestMethod]
        public void GivenUserDeleted_WhenDeleteUser_ThenReturnIsFalse()
        {
            var mock = new Mock<IUserRepository>();
            User user = new User()
            {
                Name = "Test",
                LastName = "Lasttest",
                Address = "2 rue du osef",
                Deleted = true
            };
            List<User> list = new List<User>();
            list.Add(user);

            mock.Setup(p => p.Get(It.IsAny<Expression<Func<User, Boolean>>>()))
                .Returns(list);

            var service = new UserService(mock.Object);
            var res = service.Delete(user.Id);
            Assert.IsTrue(res.Deleted);
        }
    }
}
