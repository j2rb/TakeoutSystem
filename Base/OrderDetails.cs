using System;
using System.Collections.Generic;
using System.Linq;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderDetails : IOrderDetails
    {
        private readonly TodoContext _context;

        public OrderDetails(TodoContext context)
        {
            _context = context;
        }

        public OrderDetailDTO Get(String orderCode)
        {
            Order order = _context.Orders.SingleOrDefault(o => (
                    o.OrderCode.Equals(orderCode) && o.Status == 1
                ));
            if (order != null)
            {
                IListOrderItems listOrderItems = new ListOrderItems(_context);
                List<ItemOrderDTO> items = listOrderItems.Get(order.OrderId);
                return _context.Orders
                    .Where(o => o.OrderCode.Equals(orderCode) && o.Status == 1)
                    .Select(o => new OrderDetailDTO
                    {
                        OrderCode = o.OrderCode,
                        ClientName = o.ClientName,
                        Total = items.Count,
                        Items = items
                    })
                    .ToList().First();
            }
            else
            {
                return null;
            }
        }
    }
}
