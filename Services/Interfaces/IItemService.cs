using System;
using System.Collections.Generic;
using System.Security.Claims;
using Model.Models;

namespace Services.Interfaces
{
    public interface IItemService : IGenericService<Item>
    {
        IDictionary<StockMovementType,int> GetTotalQuantity(Guid Item);
    }
}