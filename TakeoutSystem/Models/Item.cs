using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeoutSystem.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public String Name { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public Decimal Price { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
