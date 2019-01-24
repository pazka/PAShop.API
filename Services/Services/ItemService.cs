using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Model.Models;
using MoreLinq;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Services
{
    public class ItemService : GenericService<Item>, IItemService
    {
        private IGenericService<StockMovement> _stockMovementService;
        private IGenericService<Inventory> _inventoryService;

        public ItemService(IGenericRepository<Item> repo, IGenericRepository<Item> itemRepository, IGenericService<StockMovement> stockMovementService, IGenericService<Inventory> inventoryService) : base(repo)
        {
            _inventoryService = inventoryService;
            _stockMovementService = stockMovementService;
        }

        public IDictionary<StockMovementType, int> GetTotalQuantity(Guid itemId)
        {
            Inventory LastInventory = _inventoryService.Get(i => i.Item.Id == itemId).MaxBy(i => i.Timestamp)
                .SingleOrDefault();
            List<StockMovement> movements = _stockMovementService.Get(sm => sm.LastInventory.Id == LastInventory.Id);

            foreach (var curr_type in Enum.GetValues(typeof(StockMovementType)).Cast<StockMovementType>())
            {
                foreach (var stockMovementForType in movements.Where(m=>m.Type == curr_type))
                {
                    
                }
            }

            return null;//TODO
        }
    }
}
