using System;
using System.Collections.Generic;

namespace TakeoutSystem.DTO
{
    public class OrderDTO : OrderDetailsDTO
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? ServedAt { get; set; }
        public int Status { get; set; }
    }
}
