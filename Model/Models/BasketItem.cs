using System;
using System.Collections.Generic;
using System.Text;
using EFCustomAnnotations;

namespace Model.Models
{
    public class BasketItem 
    {
        [IncludeProperty]
        public Basket Basket { get; set; }
        [IncludeProperty]
        public Item Item { get; set; }

        public Guid BasketId { get; set; }
        public Guid ItemId { get; set; }

        public int Quantity { get; set; }
    }
}
