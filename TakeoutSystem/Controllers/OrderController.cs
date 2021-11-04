using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;
using AutoMapper;

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
        public ActionResult<List<OrderSimpleDTO>> GetOrder(int? Page, int? PageSize, bool? OnlyPending)
        {
            var orderRequest = new OrderRequest
            {
                Page = Page == null || Page <= 0 ? 1 : Page,
                PageSize = PageSize == null || PageSize <= 0 ? 10 : PageSize,
                OnlyPending = OnlyPending == null ? true : OnlyPending,
                Status = 1
            };
            var orders = _orderService.GetOrders(orderRequest);
            return _autoMapper.Map<List<OrderSimpleDTO>>(orders);
        }


        // GET: Order/Details
        [Route("/Order/Details")]
        [HttpGet]
        public ActionResult<OrderDetailsDTO> GetOrderDetails(String OrderCode)
        {
            var order = _orderService.GetOrder(OrderCode);
            if (order != null)
            {
                //return order;
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
        public ActionResult<OrderSimpleDTO> CreateOrder(OrderCreationRequest orderRequest)
        {
            try
            {
                var order = _orderService.Create(orderRequest);
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
        public ActionResult<OrderSimpleDTO> CancelOrder(OrderActionRequest orderActionRequest)
        {
            var order = _orderService.Cancel(orderActionRequest);
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
        public ActionResult<OrderSimpleDTO> ServeOrder(OrderActionRequest orderActionRequest)
        {
            var order = _orderService.Serve(orderActionRequest);
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
