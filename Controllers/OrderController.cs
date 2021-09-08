using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TakeoutSystem.Models;

namespace TakeoutSystem.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly TodoContext _context;

        public OrderController(TodoContext context)
        {
            _context = context;
        }

        // GET: MenuItem
        [Route("/Order")]
        [HttpGet]
        public ActionResult<IEnumerable<object>> GetOrder(Int16? Page, Int16? PageSize, Boolean? OnlyPending)
        {
            //Default values
            Page = Page == null || Page <= 0 ? 1 : Page;
            PageSize = PageSize == null || PageSize <= 0 ? 10 : PageSize;
            OnlyPending = OnlyPending == null ? true : OnlyPending;
            //Get orders
            var orders = (
                    from order in _context.Order
                    where OnlyPending.GetValueOrDefault() == true ? order.ServedTime == null : true
                    select new { OrderCode = order.OrderCode, ClientName = order.ClientName }
                )
                .Skip(Page.GetValueOrDefault() * PageSize.GetValueOrDefault() - PageSize.GetValueOrDefault())
                .Take(PageSize.GetValueOrDefault());
            return orders.ToList();
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
