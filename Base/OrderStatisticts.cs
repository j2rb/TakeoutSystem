using System;
using System.Collections.Generic;
using System.Linq;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderStatisticts : IOrderStatistics
    {
        private readonly IOrderService _orderService;

        public OrderStatisticts(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public decimal CanceledOrdersPercentage()
        {
            try
            {
                var allOrders = _orderService.GetOrders(new OrderRequest { });
                return (decimal) allOrders.Count(o => o.Status == 0) / allOrders.Count() * 100;
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }

        public decimal GetAverageItemsPerOrder()
        {
            var orders = _orderService.GetOrders(new OrderRequest { status = 1 });
            try
            {
                return (decimal) orders.Sum(o => o.Items.Sum(i => i.Quantity)) / orders.Count();
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }

        public decimal GetAverageServeTime()
        {
            try
            {
                var servedOrders = _orderService.GetOrders(new OrderRequest { status = 1, served = true });
                return (decimal) servedOrders.Sum(o => (o.ServedAt.GetValueOrDefault() - o.CreatedAt).TotalSeconds) / servedOrders.Count();
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }

        public int GetCount()
        {
           return _orderService.GetOrders(new OrderRequest { }).Count();
        }

        public List<ItemSimpleDTO> GetMostSoldItems()
        {
            return _orderService.GetOrderItems(null)
                .GroupBy(i => new { i.ItemId, i.Name })
                .OrderByDescending(i => i.Sum(i => i.Quantity))
                .Take(2)
                .Select(oi => new ItemSimpleDTO
                {
                    ItemId = oi.Key.ItemId,
                    Name = oi.Key.Name
                }).ToList();
        }
    }
}
