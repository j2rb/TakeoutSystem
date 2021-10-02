using System;
using System.Collections.Generic;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IMostSoldItems
    {
        public List<ItemSimpleDTO> Get();
    }
}
