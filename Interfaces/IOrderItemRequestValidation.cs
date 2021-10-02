using System;
using System.Collections.Generic;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderItemRequestValidation
    {
        public void Validate(List<ItemRequest> items);
    }
}
