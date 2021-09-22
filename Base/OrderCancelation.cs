using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderCancelation
    {
        private readonly TodoContext _context;

        public OrderCancelation(TodoContext context)
        {
            _context = context;
        }

        public Boolean Cancel(String orderCode)
        {
            Order order = _context.Order.SingleOrDefault(o => (
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
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
