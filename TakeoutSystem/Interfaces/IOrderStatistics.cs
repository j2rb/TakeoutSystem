using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderStatistics
    {
        public Task<List<ItemSimpleDTO>> MostSoldItems(OrderStatisticRequest orderStatisticRequest);
        public Task<decimal> AverageServeTime(OrderStatisticRequest orderStatisticRequest);
        public Task<decimal> AverageItemsPerOrder(OrderStatisticRequest orderStatisticRequest);
        public Task<decimal> CanceledOrdersPercentage(OrderStatisticRequest orderStatisticRequest);
        public Task<int> CanceledOrdersCount(OrderStatisticRequest orderStatisticRequest);
        public Task<int> TotalCount(OrderStatisticRequest orderStatisticRequest);
        public Task<decimal> TotalPriceOrders(OrderStatisticRequest orderStatisticRequest);
        public Task<decimal> AveragePriceOrders(OrderStatisticRequest orderStatisticRequest);
    }
}