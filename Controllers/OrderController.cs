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
        public List<OrderSimpleDTO> GetOrder(Int16? Page, Int16? PageSize, Boolean? OnlyPending)
        {
            ListOrders listOrders = new ListOrders(_context, _mapper);
            return listOrders.GetList(Page, PageSize, OnlyPending);
        }


        // GET: Order/Details
        [Route("/Order/Details")]
        [HttpGet]
        public ActionResult<OrderDetailDTO> GetOrderDetails(String OrderCode)
        {

            OrderDetails orderDetails = new OrderDetails(_context);
            OrderDetailDTO order = orderDetails.GetOrder(OrderCode);
            if (order != null)
            {
                return order;
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Create
        [Route("/Order")]
        [HttpPost]
        public OrderSimpleDTO CreateOrder(OrderRequest orderRequest)
        {
            OrderCreation orderCreation = new OrderCreation(_context);
            return orderCreation.Create(orderRequest);
        }

        // POST: Cancel
        [Route("/Order/Cancel")]
        [HttpPost]
        public OrderSimpleDTO CancelOrder(Order order)
        {
            OrderCancellation orderCancelation = new OrderCancellation(_context);
            return orderCancelation.Cancel(order.OrderCode);
        }

        // POST: Served
        [Route("/Order/Served")]
        [HttpPost]
        public OrderSimpleDTO ServeOrder(Order order)
        {
            OrderServe orderServe = new OrderServe(_context);
            return orderServe.Serve(order.OrderCode);
        }
    }
}
