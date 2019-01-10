using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Models
{
    public enum TvaType { Food = 25, Object = 10 };

    public class Item
    {
        public Guid Id { get; set; }
        public float Price_HT { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string ShortDesc { get; set; }
        public int Priority { get; set; }
        public int ShippingPrice { get; set; }
        public String ImageUrl { get; set; }
        public TvaType Tva { get; set; }

        public ICollection<BasketItem> Baskets { get; set; }
        public Vendor Creator { get; set; }
        public ICollection<StockMovement> StockMovements { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
    }
}
