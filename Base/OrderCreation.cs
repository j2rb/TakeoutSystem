using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderCreation : IOrderCreation
    {
        private readonly TodoContext _context;

        public OrderCreation(TodoContext context)
        {
            _context = context;
        }

        public OrderSimpleDTO Create(OrderRequest orderRequest)
        {
            IOrderRequestValidation orderRequestValidation = new OrderRequestValidation(_context);
            orderRequestValidation.Validate(orderRequest);
            IOrderCodeGenerator orderCodeGenerator = new OrderCodeGenerator();
            Order order = new Order
            {
                OrderCode = orderCodeGenerator.GetCode(),
                ClientName = orderRequest.ClientName,
                CreatedAt = DateTime.Now,
                Status = 1
            };
            _context.Orders.Add(order);
            try
            {
                _context.SaveChanges();
                IOrderItemCreation orderItemCreation = new OrderItemCreation(_context);
                orderItemCreation.Create(order.OrderId, orderRequest.Items);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException();
            }
            IOrderSimple orderSimple = new OrderSimple(_context);
            return orderSimple.Get(order.OrderCode);
        }
    }
}
