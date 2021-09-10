using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly TodoContext _context;
        private MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.CreateMap<Order, OrderSimpleDTO>());

        public OrderController(TodoContext context)
        {
            _context = context;
        }

        // GET: Order
        [Route("/Order")]
        [HttpGet]
        public List<OrderSimpleDTO> GetOrder(Int16? Page, Int16? PageSize, Boolean? OnlyPending)
        {
            //Default values
            Page = Page == null || Page <= 0 ? 1 : Page;
            PageSize = PageSize == null || PageSize <= 0 ? 10 : PageSize;
            OnlyPending = OnlyPending == null ? true : OnlyPending;
            //Get orders
            var orders = (
                    _context.Order
                    .Where(o => (OnlyPending.GetValueOrDefault() == true ? o.ServedAt == null : true) && o.Status == 1).
                    ProjectTo<OrderSimpleDTO>(configuration)
                )
                .Skip(Page.GetValueOrDefault() * PageSize.GetValueOrDefault() - PageSize.GetValueOrDefault())
                .Take(PageSize.GetValueOrDefault());
            return orders.ToList();
        }

        // POST: Cancel
        [Route("/Order/Cancel")]
        [HttpPost]
        public async Task<ActionResult<Object>> CancelOrder(Order order)
        {
            Order record = await _context.Order.SingleOrDefaultAsync(o => (
                    o.OrderCode.Equals(order.OrderCode) && o.ServedAt == null && o.Status == 1
                ));
            if (record != null)
            {
                record.Status = 0;
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
        /*
        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
        
        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }*/
    }
}
