using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderService : IOrderService
    {
        private readonly TodoContext _context;

        public OrderService(TodoContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDTO>> GetOrdersAsync(OrderRequest orderRequest)
        {
            var query = _context.Orders.AsQueryable();

            if (orderRequest.Status != null)
            {
                query = query.Where(o => o.Status == orderRequest.Status);
            }

            if (orderRequest.Served == true)
            {
                query = query.Where(o => o.ServedAt != null);
            }

            if (orderRequest.StartDate != null)
            {
                query = query.Where(o => o.CreatedAt >= orderRequest.StartDate);
            }

            if (orderRequest.EndDate != null)
            {
                query = query.Where(o => o.CreatedAt <= orderRequest.EndDate);
            }

            if (orderRequest.OnlyPending.GetValueOrDefault() == true)
            {
                query = query.Where(o => o.ServedAt == null);
            }

            if (orderRequest.Page != null)
            {
                query = query.Skip(
                    orderRequest.Page.GetValueOrDefault() * orderRequest.PageSize.GetValueOrDefault() - orderRequest.PageSize.GetValueOrDefault()
                );
            }

            if (orderRequest.PageSize != null)
            {
                query = query.Take(orderRequest.PageSize.GetValueOrDefault());
            }

            var orders = await query.Select(o => new OrderDTO
            {
                OrderCode = o.OrderCode,
                ClientName = o.ClientName,
                CreatedAt = o.CreatedAt,
                ServedAt = o.ServedAt,
                Status = o.Status
            })
                .ToListAsync();
            for (var i = 0; i < orders.Count; i++)
            {
                var orderItems = await GetOrderItemsAsync(orders[i].OrderCode);
                orders[i].Items = orderItems;
                orders[i].Total = orders[i].Items.Sum(i => i.Quantity);
            }
            return orders;
        }

        public async Task<OrderDTO> GetOrderAsync(String orderCode)
        {
            Order order = _context.Orders.SingleOrDefault(o => o.OrderCode.Equals(orderCode));
            if (order != null)
            {
                var orderItems = await GetOrderItemsAsync(orderCode);
                var result = await _context.Orders
                    .Where(o => o.OrderCode.Equals(orderCode))
                    .Select(o => new OrderDTO
                    {
                        OrderCode = o.OrderCode,
                        ClientName = o.ClientName,
                        Status = o.Status,
                        CreatedAt = o.CreatedAt,
                        ServedAt = o.ServedAt,
                        Total = orderItems.Count(),
                        Items = orderItems
                    })
                    .ToListAsync();
                return result.First();
            }
            else
            {
                return null;
            }
        }

        public async Task<List<ItemOrderDTO>> GetOrderItemsAsync(String orderCode)
        {
            return await _context.OrderItems
                .Include(oi => oi.Item)
                .Include(oi => oi.Order)
                .Where(oi => String.IsNullOrEmpty(orderCode) ? true : oi.Order.OrderCode.Equals(orderCode))
                .Select(oi => new ItemOrderDTO
                {
                    ItemId = oi.Item.ItemId,
                    Name = oi.Item.Name,
                    Price = oi.Item.Price,
                    Quantity = oi.Quantity,
                    Total = (oi.Quantity * oi.Item.Price)
                }).ToListAsync();
        }

        public async Task<OrderDTO> CreateAsync(OrderCreationRequest orderCreationRequest)
        {
            if (String.IsNullOrEmpty(orderCreationRequest.ClientName))
            {
                throw new ArgumentException("Client name is empty");
            }
            if (orderCreationRequest.Items == null || orderCreationRequest.Items.Count == 0)
            {
                throw new ArgumentException("There are no items in the order");
            }
            else
            {
                IEnumerable<int> duplicates = orderCreationRequest.Items.GroupBy(i => i.ItemId).Where(i => i.Count() > 1).Select(i => i.Key);
                if (duplicates.Count() > 0)
                {
                    throw new ArgumentException("There are duplicated items");
                }
                for (var x = 0; x < orderCreationRequest.Items.Count; x++)
                {
                    if (orderCreationRequest.Items[x].ItemId <= 0)
                    {
                        throw new ArgumentException(String.Format("[{0}] Item Id is empty or it must be > 0", x));
                    }
                    else
                    {
                        Item item = await _context.Items.SingleOrDefaultAsync(i => i.ItemId == orderCreationRequest.Items[x].ItemId);
                        if (item == null)
                        {
                            throw new ArgumentException(String.Format("[{0}] Item Id not found", x));
                        }
                    }
                    if (orderCreationRequest.Items[x].Quantity <= 0)
                    {
                        throw new ArgumentException(String.Format("[{0}] Item quantity must be > 0", x));
                    }
                }
            }
            IOrderCodeGenerator orderCodeGenerator = new OrderCodeGenerator();
            var order = new Order
            {
                OrderCode = orderCodeGenerator.GetCode(),
                ClientName = orderCreationRequest.ClientName,
                CreatedAt = DateTime.Now,
                Status = 1
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            for (var i = 0; i < orderCreationRequest.Items.Count; i++)
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.OrderId,
                    ItemId = orderCreationRequest.Items[i].ItemId,
                    Quantity = orderCreationRequest.Items[i].Quantity
                });
                await _context.SaveChangesAsync();
            }
            return await GetOrderAsync(order.OrderCode);
        }

        public async Task<OrderDTO> CancelAsync(OrderActionRequest orderActionRequest)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(o => (
                o.OrderCode.Equals(orderActionRequest.OrderCode) && o.ServedAt == null && o.Status == 1
            ));
            if (order != null)
            {
                order.Status = 0;
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return await GetOrderAsync(orderActionRequest.OrderCode);
            }
            else
            {
                return null;
            }
        }

        public async Task<OrderDTO> ServeAsync(OrderActionRequest orderActionRequest)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(o => (
                   o.OrderCode.Equals(orderActionRequest.OrderCode) && o.ServedAt == null && o.Status == 1
               ));
            if (order != null)
            {
                order.ServedAt = DateTime.Now;
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return await GetOrderAsync(orderActionRequest.OrderCode);
            }
            else
            {
                return null;
            }
        }
    }
}
