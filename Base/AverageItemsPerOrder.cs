using System;
using System.Linq;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class AverageItemsPerOrder : IAverageItemsPerOrder
    {
        private readonly TodoContext _context;

        public AverageItemsPerOrder(TodoContext context)
        {
            _context = context;
        }

        public decimal Get()
        {
            try
            {
                return (decimal)_context.OrderItems.Join(
                        _context.Orders, oi => oi.OrderId, o => o.OrderId, (orderItem, order) => new { orderItem, order }
                    )
                    .Where(oi => oi.order.Status == 1)
                    .Count() / _context.Orders.Where(o => o.Status == 1).Count();
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }
    }
}