using System;
using System.Collections.Generic;

namespace TakeoutSystem.DTO
{
    public class OrderStatisticsDTO
    {
        public List<ItemSimpleDTO> MostSoldItems { get; set; }
        public float AverageServeTimeInSeconds { get; set; }
        public float AverageItemsPerOrder { get; set; }
        public float CanceledOrdersPercentage { get; set; }
        public int TotalOrders { get; set; }
    }
}
