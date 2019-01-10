﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Models
{
    public class BasketItem
    {
        public Guid BasketId { get; set; }
        public Guid ItemId { get; set; }

        public Basket Basket { get; set; }
        public Item Item { get; set; }
    }
}
