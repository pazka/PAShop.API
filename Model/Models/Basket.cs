using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Models
{
    public class Basket : IGenericModel
    {
        public Guid Id { get; set; }
        public String State { get; set; }

        public User Owner { get; set; }
        public ICollection<Item> Items { get; set; }
        public Transaction Transaction { get; set; }
    }
}
