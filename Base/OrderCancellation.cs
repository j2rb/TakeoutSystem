using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderCancellation : IOrderCancellation
    {
        private readonly TodoContext _context;

        public OrderCancellation(TodoContext context)
        {
            _context = context;
        }

        public OrderSimpleDTO Cancel(String orderCode)
        {
            Order order = _context.Orders.SingleOrDefault(o => (
                o.OrderCode.Equals(orderCode) && o.ServedAt == null && o.Status == 1
            ));
            if (order != null)
            {
                order.Status = 0;
                _context.Entry(order).State = EntityState.Modified;
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new DbUpdateConcurrencyException();
                }
                IOrderSimple orderSimple = new OrderSimple(_context);
                return orderSimple.Get(orderCode);
            }
            else
            {
                return null;
            }
        }
    }
}
