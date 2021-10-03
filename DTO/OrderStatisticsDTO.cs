using System;
using System.Collections.Generic;

namespace TakeoutSystem.DTO
{
    public class OrderStatisticsDTO
    {
        public List<ItemSimpleDTO> MostSoldItems { get; set; }
        public decimal AverageServeTimeInSeconds { get; set; }
        public decimal AverageItemsPerOrder { get; set; }
        public decimal CanceledOrdersPercentage { get; set; }
        public int TotalOrders { get; set; }
    }
}
