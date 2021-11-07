using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderStatistics
    {
        public Task<List<ItemSimpleDTO>> MostSoldItemsAsync(OrderStatisticRequest orderStatisticRequest);
        public Task<decimal> AverageServeTimeAsync(OrderStatisticRequest orderStatisticRequest);
        public Task<decimal> AverageItemsPerOrderAsync(OrderStatisticRequest orderStatisticRequest);
        public Task<decimal> CanceledOrdersPercentageAsync(OrderStatisticRequest orderStatisticRequest);
        public Task<int> CanceledOrdersCountAsync(OrderStatisticRequest orderStatisticRequest);
        public Task<int> TotalCountAsync(OrderStatisticRequest orderStatisticRequest);
        public Task<decimal> TotalPriceOrdersAsync(OrderStatisticRequest orderStatisticRequest);
        public Task<decimal> AveragePriceOrdersAsync(OrderStatisticRequest orderStatisticRequest);
    }
}