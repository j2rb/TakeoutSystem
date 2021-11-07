using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<int> CanceledOrdersCount(OrderStatisticRequest orderStatisticRequest)
        {
            var orders = await _orderService.GetOrders(new OrderRequest
            {
                Status = 0,
                StartDate = orderStatisticRequest.StartDate,
                EndDate = orderStatisticRequest.EndDate
            });
            return orders.Count();
        }

        public async Task<decimal> CanceledOrdersPercentage(OrderStatisticRequest orderStatisticRequest)
        {
            try
            {
                var allOrders = await _orderService.GetOrders(new OrderRequest
                {
                    StartDate = orderStatisticRequest.StartDate,
                    EndDate = orderStatisticRequest.EndDate
                });
                return Math.Round((decimal)allOrders.Count(o => o.Status == 0) / allOrders.Count() * 100, 2);
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }

        public async Task<decimal> AverageItemsPerOrder(OrderStatisticRequest orderStatisticRequest)
        {
            var orders = await _orderService.GetOrders(new OrderRequest
            {
                StartDate = orderStatisticRequest.StartDate,
                EndDate = orderStatisticRequest.EndDate
            });
            try
            {
                return Math.Round((decimal)orders.Sum(o => o.Items.Sum(i => i.Quantity)) / orders.Count(), 2);
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }

        public async Task<decimal> AverageServeTime(OrderStatisticRequest orderStatisticRequest)
        {
            try
            {
                var servedOrders = await _orderService.GetOrders(new OrderRequest
                {
                    Status = 1,
                    Served = true,
                    StartDate = orderStatisticRequest.StartDate,
                    EndDate = orderStatisticRequest.EndDate
                });
                return Math.Round((decimal)servedOrders.Sum(o => (o.ServedAt.GetValueOrDefault() - o.CreatedAt).TotalSeconds) / servedOrders.Count(), 2);
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }

        public async Task<int> TotalCount(OrderStatisticRequest orderStatisticRequest)
        {
            var orders = await _orderService.GetOrders(new OrderRequest
            {
                StartDate = orderStatisticRequest.StartDate,
                EndDate = orderStatisticRequest.EndDate,
            });
            return orders.Count();
        }

        public async Task<List<ItemSimpleDTO>> MostSoldItems(OrderStatisticRequest orderStatisticRequest)
        {
            var orders = await _orderService.GetOrderItems(null);
            return orders.GroupBy(i => new { i.ItemId, i.Name })
                .OrderByDescending(i => i.Sum(i => i.Quantity))
                .Take(2)
                .Select(oi => new ItemSimpleDTO
                {
                    ItemId = oi.Key.ItemId,
                    Name = oi.Key.Name
                }).ToList();
        }

        public async Task<decimal> TotalPriceOrders(OrderStatisticRequest orderStatisticRequest)
        {
            var orders = await _orderService.GetOrders(new OrderRequest
            {
                StartDate = orderStatisticRequest.StartDate,
                EndDate = orderStatisticRequest.EndDate
            });
            return Math.Round((decimal)orders.Sum(o => o.Items.Sum(i => i.Price * i.Quantity)), 2);
        }

        public async Task<decimal> AveragePriceOrders(OrderStatisticRequest orderStatisticRequest)
        {
            try
            {
                var orders = await _orderService.GetOrders(new OrderRequest
                {
                    StartDate = orderStatisticRequest.StartDate,
                    EndDate = orderStatisticRequest.EndDate
                });
                return Math.Round((decimal)orders.Sum(o => o.Items.Sum(i => i.Price * i.Quantity)) / orders.Count(), 2);
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }
    }
}
