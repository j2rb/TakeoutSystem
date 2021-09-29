using System;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderServe
    {
        public OrderSimpleDTO Serve(String orderCode);
    }
}
