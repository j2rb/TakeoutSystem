using System;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderRequestValidation
    {
        public void Validate(OrderRequest orderRequest);
    }
}
