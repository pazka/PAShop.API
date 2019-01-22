using System;
using System.Collections.Generic;
using System.Text;
using EFCustomAnnotations;

namespace Model.Models
{
    public enum State { NotValidated, Validated, Payed, Shipped, Received}
    public class Basket : IGenericModel
    {
        public Guid Id { get; set; }
        public State State { get; set; }

        [IncludeProperty]
        public User Owner { get; set; }

        [IncludeProperty] public ICollection<Item> Items { get; set; }

        [IncludeProperty]
        public Transaction Transaction { get; set; }
    }
}
