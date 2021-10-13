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

        public int CanceledOrdersCount(OrderStatisticRequest orderStatisticRequest)
        {
            return _orderService.GetOrders(new OrderRequest
            {
                Status = 0,
                StartDate = orderStatisticRequest.StartDate,
                EndDate = orderStatisticRequest.EndDate
            }).Count();
        }

        public decimal CanceledOrdersPercentage(OrderStatisticRequest orderStatisticRequest)
        {
            try
            {
                var allOrders = _orderService.GetOrders(new OrderRequest
                {
                    StartDate = orderStatisticRequest.StartDate,
                    EndDate = orderStatisticRequest.EndDate
                });
                return (decimal)allOrders.Count(o => o.Status == 0) / allOrders.Count() * 100;
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }

        public decimal AverageItemsPerOrder(OrderStatisticRequest orderStatisticRequest)
        {
            var orders = _orderService.GetOrders(new OrderRequest
            {
                StartDate = orderStatisticRequest.StartDate,
                EndDate = orderStatisticRequest.EndDate
            });
            try
            {
                return (decimal)orders.Sum(o => o.Items.Sum(i => i.Quantity)) / orders.Count();
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }

        public decimal AverageServeTime(OrderStatisticRequest orderStatisticRequest)
        {
            try
            {
                var servedOrders = _orderService.GetOrders(new OrderRequest
                {
                    Status = 1,
                    Served = true,
                    StartDate = orderStatisticRequest.StartDate,
                    EndDate = orderStatisticRequest.EndDate
                });
                return (decimal)servedOrders.Sum(o => (o.ServedAt.GetValueOrDefault() - o.CreatedAt).TotalSeconds) / servedOrders.Count();
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }

        public int TotalCount(OrderStatisticRequest orderStatisticRequest)
        {
            return _orderService.GetOrders(new OrderRequest
            {
                StartDate = orderStatisticRequest.StartDate,
                EndDate = orderStatisticRequest.EndDate,
            }).Count();
        }

        public List<ItemSimpleDTO> MostSoldItems(OrderStatisticRequest orderStatisticRequest)
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

        public decimal TotalPriceOrders(OrderStatisticRequest orderStatisticRequest)
        {
            var orders = _orderService.GetOrders(new OrderRequest
            {
                StartDate = orderStatisticRequest.StartDate,
                EndDate = orderStatisticRequest.EndDate
            });
            return (decimal)orders.Sum(o => o.Items.Sum(i => i.Price * i.Quantity));
        }

        public decimal AveragePriceOrders(OrderStatisticRequest orderStatisticRequest)
        {
            var orders = _orderService.GetOrders(new OrderRequest
            {
                StartDate = orderStatisticRequest.StartDate,
                EndDate = orderStatisticRequest.EndDate
            });
            return (decimal)orders.Sum(o => o.Items.Sum(i => i.Price * i.Quantity)) / orders.Count();
        }
    }
}
