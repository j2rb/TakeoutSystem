using System;
using System.Collections.Generic;
using System.Linq;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderService
    {
        public List<OrderDTO> GetOrders(OrderRequest orderRequest);
        public OrderDTO GetOrder(String orderCode);
        public List<ItemOrderDTO> GetOrderItems(String orderCode);
        public OrderDTO Create(OrderCreationRequest orderCreationRequest);
        public OrderDTO Cancel(OrderActionRequest orderCreationRequest);
        public OrderDTO Serve(OrderActionRequest orderCreationRequest);
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
