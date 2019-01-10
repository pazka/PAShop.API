using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Models
{
    public class Vendor : User
    {
        public string Siret { get; set; }
        public string Brand { get; set; }

        public ICollection<Item> CreatedItems { get; set; }
    }
}
