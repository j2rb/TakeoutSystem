using System;
using System.Collections.Generic;

namespace TakeoutSystem.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public String OrderCode { get; set; }
        public String ClientName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ServedTime { get; set; }
        public int Active { get; set; }

        public List<Item> Items { get; set; }
    }
}
