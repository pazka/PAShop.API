using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public bool Deleted { get; set; }

        [Required]
        public string Email { get; set; }
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        public string Token { get; set; }

        public ICollection<Basket> Baskets { get; set; }
        public ICollection<Transaction> Payments { get; set; }
    }
}
