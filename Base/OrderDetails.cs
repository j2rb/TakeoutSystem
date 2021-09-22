using System;
using System.Collections.Generic;
using System.Linq;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderDetails
    {
        private readonly TodoContext _context;

        public OrderDetails(TodoContext context)
        {
            _context = context;
        }

        public OrderDetailDTO GetOrder(String OrderCode)
        {
            Order order = _context.Order.SingleOrDefault(o => (
                    o.OrderCode.Equals(OrderCode) && o.Status == 1
                ));
            if (order != null)
            {
                ListOrderItems listOrderItems = new ListOrderItems(_context);
                List<ItemOrderDTO> items = listOrderItems.GetList(order.OrderId);
                return _context.Order
                    .Where(o => o.OrderCode.Equals(OrderCode) && o.Status == 1)
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
