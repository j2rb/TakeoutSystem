using System;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderCancellation
    {
        public OrderSimpleDTO Cancel(String orderCode);
    }
}
