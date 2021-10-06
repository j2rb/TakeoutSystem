using System;
using System.Collections.Generic;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IItemService
    {
        public List<ItemDTO> GetItems();
    }
}
