using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
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
        private UserService _service;

        public UserServicesTester()
        { }

        [TestMethod]
        public void GivenUserNotExists_WhenAddedUser_ThenReturnIsNewUser()
        {
            var userMock = new Mock<IGenericRepository<User>>();
            var baskMock = new Mock<IGenericRepository<Basket>>();
            User user = new User()
            {
                Name = "Test",
                LastName = "Lasttest",
                Address = "2 rue du osef",
                Email = "osef@osef.com",
                Password = "ok",
                Deleted = true
            };

            userMock.Setup(r => r.Add(It.IsAny<User>()))
                .Returns(user);
            userMock.Setup(r => r.Get(It.IsAny<Expression<Func<User, Boolean>>>()))
                .Returns(new List<User>());
            baskMock.Setup(b => b.Add(It.IsAny<Basket>()))
                .Returns(new Basket());

            var service = new UserService(userMock.Object, baskMock.Object);
            var res = service.Add(user);

            Assert.AreSame(user, res);
        }

        [TestMethod]
        public void GivenUserExists_WhenAddedUser_ThenReturnIsFalse()
        {
            var userMock = new Mock<IGenericRepository<User>>();
            var baskMock = new Mock<IGenericRepository<Basket>>();
            User user = new User()
            {
                Name = "Test",
                LastName = "Lasttest",
                Address = "2 rue du osef",
                Email = "osef@osef.com",
                Password = "ok",
                Deleted = true
            };

            userMock.Setup(r => r.Add(It.IsAny<User>()))
                .Returns(user);

            userMock.Setup(r => r.Get(It.IsAny<Expression<Func<User, Boolean>>>()))
                .Returns(new List<User>()
                {
                    user
                });
            baskMock.Setup(b => b.Add(It.IsAny<Basket>()))
                .Returns(new Basket());

            var service = new UserService(userMock.Object, baskMock.Object);
            var res = service.Add(user);

            Assert.IsNull(res);
        }

        [TestMethod]
        public void GivenNoEmail_WhenAddedUser_ThenReturnIsFalse()
        {
            var userMock = new Mock<IGenericRepository<User>>();
            var baskMock = new Mock<IGenericRepository<Basket>>();
            User user = new User()
            {
                Name = "Test",
                LastName = "Lasttest",
                Address = "2 rue du osef",
                Password = "ok",
                Deleted = true
            };

            userMock.Setup(r => r.Add(It.IsAny<User>()))
                .Returns(user);

            baskMock.Setup(b => b.Add(It.IsAny<Basket>()))
                .Returns(new Basket());

            var service = new UserService(userMock.Object, baskMock.Object);
            var res = service.Add(user);

            Assert.IsNull(res);
        }


        [TestMethod]
        public void GivenGoodLogins_WhenAuthenticate_ThenReturnIsUser()
        {
            var userMock = new Mock<IGenericRepository<User>>();
            var baskMock = new Mock<IGenericRepository<Basket>>();
            User user = new User()
            {
                Name = "Test",
                LastName = "Lasttest",
                Address = "2 rue du osef",
                Email = "osef@osef.com",
                Password = "ok",
                Deleted = true
            };
            
            userMock.Setup(r => r.Get(It.IsAny<Expression<Func<User, Boolean>>>()))
                .Returns(new List<User>()
                {
                    user
                });

            var service = new UserService(userMock.Object, baskMock.Object);
            var res = service.Authenticate(user.Email, user.Password);

            Assert.AreSame(user, res);
        }

        [TestMethod]
        public void GivenBadLogins_WhenAuthenticate_ThenReturnIsUser()
        {
            var userMock = new Mock<IGenericRepository<User>>();
            var baskMock = new Mock<IGenericRepository<Basket>>();
            User user = new User()
            {
                Name = "Test",
                LastName = "Lasttest",
                Address = "2 rue du osef",
                Email = "osef@osef.com",
                Password = "ok",
                Deleted = true
            };

            userMock.Setup(r => r.Get(It.IsAny<Expression<Func<User, Boolean>>>()))
                .Returns(new List<User>());

            var service = new UserService(userMock.Object, baskMock.Object);
            var res = service.Authenticate(user.Email, user.Password);

            Assert.AreNotSame(user, res);
        }


        [TestMethod]
        public void GivenUserExists_WhenDelete_ThenReturnIsDeletedUser()
        {
            var userMock = new Mock<IGenericRepository<User>>();
            var baskMock = new Mock<IGenericRepository<Basket>>();
            User user = new User()
            {
                Id = new Guid(),
                Name = "Test",
                LastName = "Lasttest",
                Address = "2 rue du osef",
                Email = "osef@osef.com",
                Password = "ok",
                Deleted = false
            };

            userMock.Setup(r => r.Get(It.IsAny<Expression<Func<User, Boolean>>>()))
                .Returns(new List<User>()
                {
                    user
                });
            userMock.Setup(r => r.Put(It.IsAny<User>()))
                .Returns(user);

            var service = new UserService(userMock.Object, baskMock.Object);
            var res = service.Delete(user.Id);

            Assert.IsTrue(res.Deleted != user.Deleted);
        }


        [TestMethod]
        public void GivenUserNotExists_WhenDelete_ThenReturnIsNull()
        {
            var userMock = new Mock<IGenericRepository<User>>();
            var baskMock = new Mock<IGenericRepository<Basket>>();
            User user = new User()
            {
                Id = new Guid(),
                Name = "Test",
                LastName = "Lasttest",
                Address = "2 rue du osef",
                Email = "osef@osef.com",
                Password = "ok",
                Deleted = false
            };

            List<User> lu = new List<User>();
            lu.Add(user);
            userMock.Setup(r => r.Get(It.IsAny<Expression<Func<User, Boolean>>>()))
                .Returns(new List<User>());
            userMock.Setup(r => r.Put(It.IsAny<User>()))
                .Returns(user);

            var service = new UserService(userMock.Object, baskMock.Object);
            var res = service.Delete(user.Id);

            Assert.IsNull(res);
        }
    }
}
