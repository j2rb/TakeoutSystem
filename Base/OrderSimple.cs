using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderSimple
    {
        private readonly TodoContext _context;

        public OrderSimple(TodoContext context)
        {
            _context = context;
        }


        public OrderSimpleDTO Get(String orderCode)
        {
            return _context.Order
                .Where(o => o.OrderCode.Equals(orderCode))
                .Select(o => new OrderSimpleDTO
                {
                    OrderCode = o.OrderCode,
                    ClientName = o.ClientName,
                    Total = _context.OrderItem.Count(oi => oi.OrderId == o.OrderId)
                })
                .ToList().First();
        }
    }
}
