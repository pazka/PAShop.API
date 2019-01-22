using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EFCustomAnnotations;

namespace Model.Models
{
    public enum StockMovementType { Reserved, Stolen, Regular};

    public class StockMovement : IGenericModel
    {
        public Guid Id { get; set; }
        [Required]
        public int Amount { get; set; }
        public DateTime Timestamp { get; set; }
        [Required]
        public static StockMovementType Type { get; set; }

        [Required]
        [IncludeProperty]
        public Item Item { get; set; }
        [IncludeProperty]
        public Inventory LastInventory { get; set; }
    }
}
