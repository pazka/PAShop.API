using System;
using System.Security.Claims;
using Model.Models;

namespace Services.Interfaces
{
    public interface IBasketService : IGenericService<Basket>
    {
        Basket Mine(ClaimsPrincipal claimsPrincipal);
        double GetTotalPrice(Basket basket);
    }
}