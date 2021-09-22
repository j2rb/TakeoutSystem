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
        public Boolean CancelOrder(Order order)
        {
            OrderCancelation orderCancelation = new OrderCancelation(_context);
            return orderCancelation.Cancel(order.OrderCode);
        }

        // POST: Served
        [Route("/Order/Served")]
        [HttpPost]
        public async Task<ActionResult<Object>> ServeOrder(Order order)
        {
            Order record = await _context.Order.SingleOrDefaultAsync(o => (
                    o.OrderCode.Equals(order.OrderCode) && o.ServedAt == null && o.Status == 1
                ));
            if (record != null)
            {
                record.ServedAt = DateTime.Now;
                _context.Entry(record).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return StatusCode(500);
                }
                return new { Status = "Successful" };
            }
            else
            {
                return NotFound();
            }
        }
    }
}
