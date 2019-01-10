using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Models
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }

        public Item Item { get; set; }
        public ICollection<StockMovement> StockMovements { get; set; }
    }
}
