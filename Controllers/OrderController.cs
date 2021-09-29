using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TakeoutSystem.Base;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public OrderController(TodoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Order
        [Route("/Order")]
        [HttpGet]
        public ActionResult<List<OrderSimpleDTO>> GetOrder(Int16? Page, Int16? PageSize, Boolean? OnlyPending)
        {
            try
            {
                IListOrders listOrders = new ListOrders(_context);
                return listOrders.Get(Page, PageSize, OnlyPending);
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
                IOrderDetails orderDetails = new OrderDetails(_context);
                OrderDetailDTO order = orderDetails.Get(OrderCode);
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
        public ActionResult<OrderSimpleDTO> CreateOrder(OrderRequest orderRequest)
        {
            try
            {
                IOrderCreation orderCreation = new OrderCreation(_context);
                return orderCreation.Create(orderRequest);
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
        public ActionResult<OrderSimpleDTO> CancelOrder(Order order)
        {
            try
            {
                IOrderCancellation orderCancelation = new OrderCancellation(_context);
                OrderSimpleDTO response = orderCancelation.Cancel(order.OrderCode);
                if (response != null)
                {
                    return response;
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
        public ActionResult<OrderSimpleDTO> ServeOrder(Order order)
        {
            try
            {
                IOrderServe orderServe = new OrderServe(_context);
                OrderSimpleDTO response = orderServe.Serve(order.OrderCode);
                if (response != null)
                {
                    return response;
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
