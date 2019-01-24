using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EFCustomAnnotations;

namespace Model.Models
{
    public enum TvaType { Food = 25, Object = 10 };

    public class Item : IGenericModel
    {
        public Guid Id { get; set; }
        [Required]
        public float Price_HT { get; set; }
        [Required]
        public string Label { get; set; }
        public string Description { get; set; }
        [Required]
        public string ShortDesc { get; set; }
        [Required]
        public int Priority { get; set; }
        [Required]
        public int ShippingPrice { get; set; }
        public String ImageUrl { get; set; }
        [Required]
        public TvaType Tva { get; set; }
        public bool Deleted { get; set; }
        
        public ICollection<StockMovement> StockMovements { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
    }
}
