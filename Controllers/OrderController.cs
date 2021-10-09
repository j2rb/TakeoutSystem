using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly AutoMapper _autoMapper;

        public OrderController(IOrderService orderService, AutoMapper autoMapper)
        {
            _orderService = orderService;
            _autoMapper = autoMapper;
        }

        // GET: Order
        [Route("/Order")]
        [HttpGet]
        public ActionResult<List<OrderSimpleDTO>> GetOrder(int? Page, int? PageSize, bool? OnlyPending)
        {
            try
            {
                var orderRequest = new OrderRequest {
                    Page = Page == null || Page <= 0 ? 1 : Page,
                    PageSize = PageSize == null || PageSize <= 0 ? 10 : PageSize,
                    OnlyPending = OnlyPending == null ? true : OnlyPending,
                    Status = 1
                };
                var orders = _orderService.GetOrders(orderRequest);

                _autoMapper.Map<Source, Destination>(new Source { Value = 15 });


                return orders.Select(o => new OrderSimpleDTO
                {
                    OrderCode = o.OrderCode,
                    ClientName = o.ClientName,
                    Total = o.Total,
                }).ToList();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        // GET: Order/Details
        [Route("/Order/Details")]
        [HttpGet]
        public ActionResult<OrderDetailDTO> GetOrderDetails(String OrderCode)
        {
            try
            {
                var order = _orderService.GetOrder(OrderCode);
                if (order != null)
                {
                    return order;
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // POST: Create
        [Route("/Order")]
        [HttpPost]
        public ActionResult<OrderSimpleDTO> CreateOrder(OrderCreationRequest orderRequest)
        {
            try
            {
                return _orderService.Create(orderRequest);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // POST: Cancel
        [Route("/Order/Cancel")]
        [HttpPost]
        public ActionResult<OrderDetailDTO> CancelOrder(OrderActionRequest orderActionRequest)
        {
            try
            {
                var order = _orderService.Cancel(orderActionRequest);
                if (order != null)
                {
                    return order;
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // POST: Served
        [Route("/Order/Served")]
        [HttpPost]
        public ActionResult<OrderDetailDTO> ServeOrder(OrderActionRequest orderActionRequest)
        {
            try
            {
                var order = _orderService.Serve(orderActionRequest);
                if (order != null)
                {
                    return order;
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
