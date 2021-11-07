using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IItemService
    {
        public Task<List<ItemDTO>> GetItems();
    }
}
