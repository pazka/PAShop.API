using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Model.Models
{
    public enum Role { Admin, LoggedUser }
    public class User : IdentityUser
    {
        public new Guid Id { get; set; }
        public bool Deleted { get; set; }

        [Required]
        public new string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        public Role Role { get; set; }
        public string Token { get; set; }

        public ICollection<Basket> Baskets { get; set; }
        public ICollection<Transaction> Payments { get; set; }
    }
}
