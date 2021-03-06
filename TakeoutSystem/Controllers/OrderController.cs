using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;
using AutoMapper;
using System.Threading.Tasks;

namespace TakeoutSystem.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _autoMapper;

        public OrderController(IOrderService orderService, IMapper autoMapper)
        {
            _orderService = orderService;
            _autoMapper = autoMapper;
        }

        // GET: Order
        [Route("/Order")]
        [HttpGet]
        public async Task<ActionResult<List<OrderSimpleDTO>>> GetOrder([FromQuery] OrderRequest orderRequest)
        {
            orderRequest.OnlyPending = orderRequest.OnlyPending.GetValueOrDefault(true);
            orderRequest.Status = 1;
            var orders = await _orderService.GetOrdersAsync(orderRequest);
            return _autoMapper.Map<List<OrderSimpleDTO>>(orders);
        }


        // GET: Order/Details
        [Route("/Order/Details")]
        [HttpGet]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetails([FromQuery] OrderRequest orderRequest)
        {
            var order = await _orderService.GetOrderAsync(orderRequest);
            if (order != null)
            {
                return _autoMapper.Map<OrderDetailsDTO>(order);
            }
            else
            {
                return Problem(title: "Order not found", detail: "Order not found with code sent", statusCode: 404);
            }
        }

        // POST: Create
        [Route("/Order")]
        [HttpPost]
        public async Task<ActionResult<OrderSimpleDTO>> CreateOrder(OrderCreationRequest orderRequest)
        {
            try
            {
                var order = await _orderService.CreateAsync(orderRequest);
                return _autoMapper.Map<OrderSimpleDTO>(order);
            }
            catch (ArgumentException e)
            {
                return Problem(title: "Wrong argument in request", detail: e.Message, statusCode: 400);
            }
        }

        // POST: Cancel
        [Route("/Order/Cancel")]
        [HttpPost]
        public async Task<ActionResult<OrderSimpleDTO>> CancelOrder(OrderActionRequest orderActionRequest)
        {
            var order = await _orderService.CancelAsync(orderActionRequest);
            if (order != null)
            {
                return _autoMapper.Map<OrderSimpleDTO>(order);
            }
            else
            {
                return Problem(title: "Order not found", detail: "Order not found with code sent", statusCode: 404);
            }
        }

        // POST: Served
        [Route("/Order/Served")]
        [HttpPost]
        public async Task<ActionResult<OrderSimpleDTO>> ServeOrder(OrderActionRequest orderActionRequest)
        {
            var order = await _orderService.ServeAsync(orderActionRequest);
            if (order != null)
            {
                return _autoMapper.Map<OrderSimpleDTO>(order);
            }
            else
            {
                return Problem(title: "Order not found", detail: "Order not found with code sent", statusCode: 404);
            }
        }
    }
}
