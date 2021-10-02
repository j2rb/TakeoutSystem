using System;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderRequestValidation : IOrderRequestValidation
    {
        private readonly TodoContext _context;

        public OrderRequestValidation(TodoContext context)
        {
            _context = context;
        }

        public void Validate(OrderRequest orderRequest)
        {
            if (String.IsNullOrEmpty(orderRequest.ClientName))
            {
                throw new ArgumentException();
            }
            IOrderItemRequestValidation orderItemRequestValidation = new OrderItemRequestValidation(_context);
            orderItemRequestValidation.Validate(orderRequest.Items);
        }
    }
}
