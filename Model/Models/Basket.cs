using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EFCustomAnnotations;

namespace Model.Models
{
    public enum BasketState { NotValidated, Validated, Payed, Shipped, Received}
    public class Basket : IGenericModel
    {
        public Guid Id { get; set; }
        
        public BasketState State { get; set; }

        [IncludeProperty]
        public User Owner { get; set; }

        [IncludeProperty]
        public ICollection<BasketItem> BasketItems { get; set; }

        [IncludeProperty]
        public Transaction Transaction { get; set; }
    }
}
