using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Models
{
    public enum StockMovementType { Reserved, Stolen, Regular};

    public class StockMovement : IGenericModel
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public static StockMovementType Type { get; set; }

        public Item Item { get; set; }
        public Inventory LastInventory { get; set; }
    }
}
