using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Models
{
    public class Inventory : IGenericModel
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }

        public Item Item { get; set; }
        public DateTime Timestamp { get; set; }
        public ICollection<StockMovement> StockMovements { get; set; }
    }
}
