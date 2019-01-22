using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EFCustomAnnotations;

namespace Model.Models
{
    public class Transaction : IGenericModel
    {
        public Guid Id { get; set; }
        public String State { get; set; }

        [IncludeProperty]
        public Basket Order { get; set; }
        public Guid OrderId { get; set; }
        [IncludeProperty]
        public User User { get; set; }
    }
}
