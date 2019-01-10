using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public String State { get; set; }

        public Basket Order { get; set; }
        public User User { get; set; }
    }
}
