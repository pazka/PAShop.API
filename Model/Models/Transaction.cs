using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EFCustomAnnotations;

namespace Model.Models
{
    public enum TransactionState
    {
        Pending,
        Payed
    };
    public class Transaction : IGenericModel
    {
        public Guid Id { get; set; }
        public TransactionState State { get; set; }

        [IncludeProperty]
        public Basket Order { get; set; }
        public Guid OrderId { get; set; }

        public User Owner { get; set; }
        public Guid OwnerId { get; set; }
    }
}
