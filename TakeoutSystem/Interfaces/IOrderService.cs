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
        public Task<List<OrderDTO>> GetOrdersAsync(OrderRequest orderRequest);
        public Task<OrderDTO> GetOrderAsync(OrderRequest orderRequest);
        public Task<List<ItemOrderDTO>> GetOrderItemsAsync(String orderCode);
        public Task<OrderDTO> CreateAsync(OrderCreationRequest orderCreationRequest);
        public Task<OrderDTO> CancelAsync(OrderActionRequest orderCreationRequest);
        public Task<OrderDTO> ServeAsync(OrderActionRequest orderCreationRequest);
    }
}
