using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.Models
{
    public class Transaction : IGenericModel
    {
        public Guid Id { get; set; }
        public String State { get; set; }

        public Basket Order { get; set; }
        public Guid OrderId { get; set; }
        public User User { get; set; }
    }
}
