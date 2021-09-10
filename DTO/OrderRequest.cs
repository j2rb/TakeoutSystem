using System;
using System.Collections.Generic;

namespace TakeoutSystem.DTO
{
    public class OrderRequest
    {
        public String ClientName { get; set; }
        public List<ItemRequest> Items { get; set; } 
    }
}
