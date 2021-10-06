using System;
using System.Collections.Generic;
using System.Linq;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderService
    {
        public List<OrderDetailDTO> GetOrders(OrderRequest orderRequest);
        public OrderDetailDTO GetOrder(String orderCode);
        public List<ItemOrderDTO> GetOrderItems(String orderCode);
        public OrderDetailDTO Create(OrderCreationRequest orderCreationRequest);
        public OrderDetailDTO Cancel(OrderActionRequest orderCreationRequest);
        public OrderDetailDTO Serve(OrderActionRequest orderCreationRequest);
    }
   
    /*
    public class StatisticService
    {
        private IOrderService _orderService = null;

        public StatisticService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public int GetOrderCount(OrderRequest orderRequest)
        {
           var orders = _orderService.GetOrders(orderRequest);
           return orders.Count();
        }
    }
    */
}
