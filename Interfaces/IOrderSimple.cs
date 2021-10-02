using System;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderSimple
    {
        public OrderSimpleDTO Get(String orderCode);
    }
}
