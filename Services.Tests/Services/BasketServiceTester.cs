using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Models;
using Moq;
using Repositories.Interfaces;
using Services.Services;

namespace Services.Tests.Services
{
    [TestClass]
    public class BasketServiceTester
    {
        private BasketService _basketService;

        public BasketServiceTester()
        {

        }

        [TestMethod]
        public void GivenUserNotExists_WhenMine_ThenReturnEmptyBasket()
        {
            var userMock = new Mock<IGenericRepository<User>>();
            var itemMock = new Mock<IGenericRepository<Item>>();
            var baskMock = new Mock<IGenericRepository<Basket>>();
            var claimMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(u => u.Get(It.IsAny<Expression<Func<User, Boolean>>>()))
                .Returns(new List<User>());

            baskMock.Setup(u => u.Get(It.IsAny<Expression<Func<Basket, Boolean>>>()))
                .Returns(new List<Basket>()
                {
                    new Basket()
                    {
                        BasketItems = new List<BasketItem>(){
                            new BasketItem()

                        }
                    }
                });

            List<Item> items = new List<Item>();
            items.Add(new Item()
            {
                Label = "Test"
            });
            itemMock.Setup(i => i.Get(It.IsAny<Expression<Func<Item, Boolean>>>()))
                .Returns(items);

            claimMock.Setup(c => c.FindFirst(It.IsAny<Predicate<Claim>>()))
                .Returns(new Claim("", ""));

            _basketService = new BasketService(baskMock.Object, userMock.Object, itemMock.Object);

            Assert.IsNull(_basketService.Mine(claimMock.Object).BasketItems);
        }

        [TestMethod]
        public void GivenBasketHasItems_WhenMine_ThenReturnItemBasket()
        {
            var userMock = new Mock<IGenericRepository<User>>();
            var itemMock = new Mock<IGenericRepository<Item>>();
            var baskMock = new Mock<IGenericRepository<Basket>>();
            var claimMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(u => u.Get(It.IsAny<Expression<Func<User, Boolean>>>()))
                .Returns(new List<User>()
                {
                    new User()
                    {
                        Email = "osef"
                    }
                });

            baskMock.Setup(u => u.Get(It.IsAny<Expression<Func<Basket, Boolean>>>()))
                .Returns(new List<Basket>()
                {
                    new Basket()
                    {
                        BasketItems = new List<BasketItem>(){
                            new BasketItem(),
                            new BasketItem(),
                            new BasketItem()
                        }
                    }
                });

            List<Item> items = new List<Item>();
            items.Add(new Item()
            {
                Label = "Test"
            });
            itemMock.Setup(i => i.Get(It.IsAny<Expression<Func<Item, Boolean>>>()))
                .Returns(items);

            claimMock.Setup(c => c.FindFirst(It.IsAny<Predicate<Claim>>()))
                .Returns(new Claim("", ""));

            _basketService = new BasketService(baskMock.Object, userMock.Object, itemMock.Object);

            Assert.IsTrue(_basketService.Mine(claimMock.Object).BasketItems.Count == 3);
        }
    }
}
