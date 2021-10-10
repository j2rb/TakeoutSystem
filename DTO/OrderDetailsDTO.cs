using System;
using System.Collections.Generic;

namespace TakeoutSystem.DTO
{
    public class OrderDetailsDTO : OrderSimpleDTO
    {
        public List<ItemOrderDTO> Items { get; set; }
    }
}
