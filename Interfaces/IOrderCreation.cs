using System;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderCreation
    {
        public OrderSimpleDTO Create(OrderRequest order);
    }
}
