using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public Dictionary<StockMovementType, int> GetGlobalQuantity(Guid itemId)
        {
            Dictionary<StockMovementType,int> res = new Dictionary<StockMovementType, int>();

            Inventory lastInventory = _inventoryService.Get(i => i.Item.Id == itemId).MaxBy(i => i.Timestamp)
                .SingleOrDefault();
            List<StockMovement> movements = _stockMovementService.Get(sm => sm.LastInventory.Id == lastInventory.Id);

            int tmp;
            foreach (StockMovementType curr_type in Enum.GetValues(typeof(StockMovementType)).Cast<StockMovementType>())
            {
                tmp = curr_type == StockMovementType.Regular? lastInventory.Quantity:0;

                foreach (var stockMovementForType in movements.Where(m=>m.Type == curr_type))
                {
                    tmp += stockMovementForType.Amount;
                }

                res.Add(curr_type, tmp);
            }

            return res;
        }

        public new List<Item> Get(Expression<Func<Item, bool>> predicate)
        {
            return _repository.Get(predicate).Where(i => i.Deleted == false).ToList();
        }

        public new Item Delete(Guid id)
        {
            Item item = _repository.Get(i => i.Id == id).SingleOrDefault();
            item.Deleted = !item.Deleted;
            return _repository.Put(item);
        }
    }
}
