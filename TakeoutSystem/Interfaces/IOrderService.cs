using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderService
    {
        public Task<List<OrderDTO>> GetOrders(OrderRequest orderRequest);
        public Task<OrderDTO> GetOrder(String orderCode);
        public Task<List<ItemOrderDTO>> GetOrderItems(String orderCode);
        public Task<OrderDTO> Create(OrderCreationRequest orderCreationRequest);
        public Task<OrderDTO> Cancel(OrderActionRequest orderCreationRequest);
        public Task<OrderDTO> Serve(OrderActionRequest orderCreationRequest);
    }
}
