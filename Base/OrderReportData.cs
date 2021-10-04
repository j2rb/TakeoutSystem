using System;
using System.Collections.Generic;
using System.Linq;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderReportData : IOrderReportData
    {
        private readonly TodoContext _context;

        public OrderReportData(TodoContext context)
        {
            _context = context;
        }

        public List<OrderSimpleDTO> Get(DateTime startDate, DateTime endDate)
        {
            return _context.Orders
                    .Join(
                        _context.OrderItems, o => o.OrderId, oi => oi.OrderId, (order, orderItem) => new { order, orderItem }
                    )
                    .Where(o => o.order.Status == 1 && o.order.CreatedAt >= startDate && o.order.CreatedAt <= endDate)
                    .GroupBy(o => new { o.order.OrderCode, o.order.ClientName, o.order.CreatedAt })
                    .OrderBy(o => o.Key.CreatedAt)
                    .Select(o => new OrderSimpleDTO
                    {
                        OrderCode = o.Key.OrderCode,
                        ClientName = o.Key.ClientName,
                        Total = o.Count()
                    })
                    .ToList();
        }
    }
}
