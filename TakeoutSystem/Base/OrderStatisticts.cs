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

        public async Task<int> CanceledOrdersCountAsync(OrderStatisticRequest orderStatisticRequest)
        {
            var orders = await _orderService.GetOrdersAsync(new OrderRequest
            {
                Status = 0,
                StartDate = orderStatisticRequest.StartDate,
                EndDate = orderStatisticRequest.EndDate
            });
            return orders.Count();
        }

        public async Task<decimal> CanceledOrdersPercentageAsync(OrderStatisticRequest orderStatisticRequest)
        {
            try
            {
                var allOrders = await _orderService.GetOrdersAsync(new OrderRequest
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

        public async Task<decimal> AverageItemsPerOrderAsync(OrderStatisticRequest orderStatisticRequest)
        {
            var orders = await _orderService.GetOrdersAsync(new OrderRequest
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

        public async Task<decimal> AverageServeTimeAsync(OrderStatisticRequest orderStatisticRequest)
        {
            try
            {
                var servedOrders = await _orderService.GetOrdersAsync(new OrderRequest
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

        public async Task<int> TotalCountAsync(OrderStatisticRequest orderStatisticRequest)
        {
            var orders = await _orderService.GetOrdersAsync(new OrderRequest
            {
                StartDate = orderStatisticRequest.StartDate,
                EndDate = orderStatisticRequest.EndDate,
            });
            return orders.Count();
        }

        public async Task<List<ItemSimpleDTO>> MostSoldItemsAsync(OrderStatisticRequest orderStatisticRequest)
        {
            var orders = await _orderService.GetOrderItemsAsync(null);
            return orders.GroupBy(i => new { i.ItemId, i.Name })
                .OrderByDescending(i => i.Sum(i => i.Quantity))
                .Take(2)
                .Select(oi => new ItemSimpleDTO
                {
                    ItemId = oi.Key.ItemId,
                    Name = oi.Key.Name
                }).ToList();
        }

        public async Task<decimal> TotalPriceOrdersAsync(OrderStatisticRequest orderStatisticRequest)
        {
            var orders = await _orderService.GetOrdersAsync(new OrderRequest
            {
                StartDate = orderStatisticRequest.StartDate,
                EndDate = orderStatisticRequest.EndDate
            });
            return Math.Round((decimal)orders.Sum(o => o.Items.Sum(i => i.Price * i.Quantity)), 2);
        }

        public async Task<decimal> AveragePriceOrdersAsync(OrderStatisticRequest orderStatisticRequest)
        {
            try
            {
                var orders = await _orderService.GetOrdersAsync(new OrderRequest
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
