using System;
using System.Collections.Generic;

namespace TakeoutSystem.DTO
{
    public class OrderDetailDTO : OrderSimpleDTO
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? ServedAt { get; set; }
        public int Status { get; set; }
        public List<ItemOrderDTO> Items { get; set; }
    }
}
