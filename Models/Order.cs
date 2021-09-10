using System;
using System.Collections.Generic;

namespace TakeoutSystem.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public String OrderCode { get; set; }
        public String ClientName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ServedAt { get; set; }
        public Byte Status { get; set; }

        public OrderItem OrderItem { get; set; }
    }
}
