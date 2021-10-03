using System;
using System.Collections.Generic;
using System.Linq;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class MostSoldItems : IMostSoldItems
    {
        private readonly TodoContext _context;

        public MostSoldItems(TodoContext context)
        {
            _context = context;
        }

        public List<ItemSimpleDTO> Get()
        {
            return _context.OrderItems.Join(
                    _context.Items, oi => oi.ItemId, i => i.ItemId, (orderItem, item) => new { orderItem, item }
                )
                .Join(
                    _context.Orders, oi => oi.orderItem.OrderId, o => o.OrderId, (orderItem, order) => new { orderItem.orderItem, orderItem.item, order }
                )
                .Where(oi => oi.order.Status == 1)
                .GroupBy(oi => new { oi.item.ItemId, oi.item.Name })
                .OrderByDescending(oi => oi.Sum(oi => oi.orderItem.Quantity))
                .Take(2)
                .Select(oi => new ItemSimpleDTO
                {
                    ItemId = oi.Key.ItemId,
                    Name = oi.Key.Name
                }).ToList();
        }
    }
}
