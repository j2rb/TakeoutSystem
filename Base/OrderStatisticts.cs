using System;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderStatisticts : IOrderStatistics
    {
        private readonly TodoContext _context;

        public OrderStatisticts(TodoContext context)
        {
            _context = context;
        }

        public OrderStatisticsDTO Get()
        {
            IMostSoldItems mostSoldItems = new MostSoldItems(_context);
            IAverageServeTime averageServeTime = new AverageServeTime(_context);
            IAverageItemsPerOrder averageItemsPerOrder = new AverageItemsPerOrder(_context);
            ICanceledOrdersPercentage canceledOrdersPercentage = new CanceledOrdersPercentage(_context);
            ITotalOrders totalOrders = new TotalOrders(_context);
            return new OrderStatisticsDTO {
                MostSoldItems = mostSoldItems.Get(),
                AverageServeTimeInSeconds = Math.Round(averageServeTime.Get(), 2),
                AverageItemsPerOrder = Math.Round(averageItemsPerOrder.Get(), 2),
                CanceledOrdersPercentage = Math.Round(canceledOrdersPercentage.Get(), 2),
                TotalOrders = totalOrders.Get()
            };
        }
    }
}
