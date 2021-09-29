using System;
using System.Collections.Generic;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IListOrderItems
    {
        public List<ItemOrderDTO> Get(int orderId);
    }
}
