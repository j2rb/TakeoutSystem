using System;
using System.Collections.Generic;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderStatistics
    {
        public List<ItemSimpleDTO> GetMostSoldItems();
        public decimal GetAverageServeTime();
        public decimal GetAverageItemsPerOrder();
        public decimal CanceledOrdersPercentage();
        public int GetCount();
    }
}