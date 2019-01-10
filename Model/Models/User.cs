using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool   Deleted { get; set; }

        public ICollection<Basket> Baskets { get; set; }
        public ICollection<Transaction> Payments { get; set; }
    }
}
