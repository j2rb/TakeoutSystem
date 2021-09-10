using System;
using System.Collections.Generic;
namespace TakeoutSystem.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public String Name { get; set; }
        public Decimal Price { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
