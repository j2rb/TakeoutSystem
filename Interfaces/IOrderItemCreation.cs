using System;
using System.Collections.Generic;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderItemCreation
    {
        public void Create(int orderId, List<ItemRequest> itemRequest);
    }
}
