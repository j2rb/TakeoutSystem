using System;
using System.Collections.Generic;

namespace TakeoutSystem.DTO
{
    public class OrderDetailDTO : OrderSimpleDTO
    {
        public List<ItemOrderDTO> Items { get; set; }
    }
}
