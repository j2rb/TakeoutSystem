using System;
using System.Collections.Generic;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IListOrders
    {
        public List<OrderSimpleDTO> Get(Int16? Page, Int16? PageSize, Boolean? OnlyPending);
    }
}
