using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TakeoutSystem.Models
{
    [Index(nameof(OrderCode)), Index(nameof(CreatedAt)), Index(nameof(ServedAt))]
    public class Order
    {
        public int OrderId { get; set; }
        public String OrderCode { get; set; }
        public String ClientName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ServedAt { get; set; }
        public Byte Status { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
