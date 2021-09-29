using System;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderDetails
    {
        public OrderDetailDTO Get(String orderCode);
    }
}
