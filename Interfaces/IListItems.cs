using System;
using System.Collections.Generic;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IListItems
    {
        public List<ItemDTO> Get();
    }
}
