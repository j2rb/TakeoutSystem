using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderServe
    {
        private readonly TodoContext _context;

        public OrderServe(TodoContext context)
        {
            _context = context;
        }

        public OrderSimpleDTO Serve(String orderCode)
        {
            Order order =  _context.Order.SingleOrDefault(o => (
                    o.OrderCode.Equals(orderCode) && o.ServedAt == null && o.Status == 1
                ));
            if (order != null)
            {
                order.ServedAt = DateTime.Now;
                _context.Entry(order).State = EntityState.Modified;
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new DbUpdateConcurrencyException();
                }
                OrderSimple orderSimple = new OrderSimple(_context);
                return orderSimple.Get(orderCode);
            }
            else
            {
                return null;
            }
        }
    }
}
