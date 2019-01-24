using System;
using System.Collections.Generic;
using System.Security.Claims;
using Model.Models;

namespace Services.Interfaces
{
    public interface IItemService : IGenericService<Item>
    {
        Dictionary<StockMovementType,int> GetGlobalQuantity(Guid Item);
    }
}