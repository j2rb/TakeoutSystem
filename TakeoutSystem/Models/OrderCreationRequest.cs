using System;
using System.Collections.Generic;

namespace TakeoutSystem.Models
{
    public class OrderCreationRequest
    {
        public String ClientName { get; set; }
        public List<OrderItemCreationRequest> Items { get; set; }
    }
}
