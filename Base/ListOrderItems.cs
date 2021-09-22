using System;
using System.Collections.Generic;
using System.Linq;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class ListOrderItems
    {
        private readonly TodoContext _context;

        public ListOrderItems(TodoContext context)
        {
            _context = context;
        }

        public List<ItemOrderDTO> GetList(int OrderId)
        {
            return _context.OrderItem
                .Join(
                    _context.Items, oi => oi.ItemId, i => i.ItemId, (orderItem, item) => new { orderItem, item }
                )
                .Where(oi => oi.orderItem.OrderId == OrderId)
                .Select(oi => new ItemOrderDTO {
                    ItemId = oi.item.ItemId,
                    Name = oi.item.Name,
                    Price = oi.item.Price,
                    Quantity = oi.orderItem.Quantity,
                    Total = (oi.orderItem.Quantity * oi.item.Price)
                }).ToList();
        }
    }
}
