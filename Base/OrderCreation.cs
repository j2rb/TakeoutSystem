using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderCreation
    {
        private readonly TodoContext _context;

        public OrderCreation(TodoContext context)
        {
            _context = context;
        }

        public OrderSimpleDTO Create(OrderRequest orderRequest)
        {
            if (String.IsNullOrEmpty(orderRequest.ClientName))
            {
                throw new ArgumentException();
            }
            if (orderRequest.Items == null || orderRequest.Items.Count == 0)
            {
                throw new ArgumentException();
            }
            else
            {
                for (var x = 0; x < orderRequest.Items.Count; x++)
                {
                    if (orderRequest.Items[x].ItemId <= 0)
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        IEnumerable<short> duplicates = orderRequest.Items.GroupBy(i => i.ItemId).Where(i => i.Count() > 1).Select(i => i.Key);
                        if (duplicates.Count() > 0)
                        {
                            throw new ArgumentException();
                        }

                        Item item = _context.Items.SingleOrDefault(i => i.ItemId == orderRequest.Items[x].ItemId);
                        if (item == null)
                        {
                            throw new ArgumentException();
                        }
                    }
                    if (orderRequest.Items[x].Quantity <= 0)
                    {
                        throw new ArgumentException();
                    }
                }
            }

            Guid orderCode = Guid.NewGuid();
            Order order = new Order
            {
                OrderCode = orderCode.ToString(),
                ClientName = orderRequest.ClientName,
                CreatedAt = DateTime.Now,
                Status = 1
            };
            _context.Order.Add(order);
            try
            {
                _context.SaveChanges();
                OrderItemCreation orderItemCreation = new OrderItemCreation(_context);
                orderItemCreation.Create(order.OrderId, orderRequest.Items);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException();
            }
            OrderSimple orderSimple = new OrderSimple(_context);
            return orderSimple.Get(order.OrderCode);
        }
    }
}
