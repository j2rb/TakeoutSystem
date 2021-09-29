using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;
namespace TakeoutSystem.Base
{
    public class OrderItemCreation : IOrderItemCreation
    {
        private readonly TodoContext _context;

        public OrderItemCreation(TodoContext context)
        {
            _context = context;
        }

        public void Create(int orderId, List<ItemRequest> itemRequest)
        {
            try
            {
                for (var i = 0; i < itemRequest.Count; i++)
                {
                    _context.OrderItem.Add(new OrderItem
                    {
                        OrderId = orderId,
                        ItemId = itemRequest[i].ItemId,
                        Quantity = itemRequest[i].Quantity
                    });
                }
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException();
            }
        }
    }
}
