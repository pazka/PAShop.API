using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EFCustomAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Model.Models
{
    public enum Role{Admin = 2,LoggedUser = 0,Vendor = 1}

    public class User : IGenericModel
    {
        public Guid Id { get; set; }
        public bool Deleted { get; set; }

        [Required]
        public  string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [DefaultValue(Role.LoggedUser)]
        public Role Role { get; set; }
        public string Token { get; set; }

        [IncludeProperty]
        public ICollection<Basket> Baskets { get; set; }

        public ICollection<Transaction> Payments { get; set; }
    }
}
