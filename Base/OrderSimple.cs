using System;
using System.Linq;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderSimple : IOrderSimple
    {
        private readonly TodoContext _context;

        public OrderSimple(TodoContext context)
        {
            _context = context;
        }

        public OrderSimpleDTO Get(String orderCode)
        {
            return _context.Orders
                .Where(o => o.OrderCode.Equals(orderCode))
                .Select(o => new OrderSimpleDTO
                {
                    OrderCode = o.OrderCode,
                    ClientName = o.ClientName,
                    Total = _context.OrderItems.Count(oi => oi.OrderId == o.OrderId)
                })
                .ToList().First();
        }
    }
}
