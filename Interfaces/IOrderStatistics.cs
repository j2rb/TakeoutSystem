using System;
using System.Collections.Generic;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderStatistics
    {
        public List<ItemSimpleDTO> MostSoldItems(OrderStatisticRequest orderStatisticRequest);
        public decimal AverageServeTime(OrderStatisticRequest orderStatisticRequest);
        public decimal AverageItemsPerOrder(OrderStatisticRequest orderStatisticRequest);
        public decimal CanceledOrdersPercentage(OrderStatisticRequest orderStatisticRequest);
        public int CanceledOrdersCount(OrderStatisticRequest orderStatisticRequest);
        public int TotalCount(OrderStatisticRequest orderStatisticRequest);
        public decimal TotalPriceOrders(OrderStatisticRequest orderStatisticRequest);
        public decimal AveragePriceOrders(OrderStatisticRequest orderStatisticRequest);
    }
}