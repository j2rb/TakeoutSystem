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
    public class MenuItemController : ControllerBase
    {
        private readonly TodoContext _context;

        public MenuItemController(TodoContext context)
        {
            _context = context;
        }

        // GET: MenuItem
        [Route("/MenuItem")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            Console.WriteLine(await _context.Items.ToListAsync());
            return await _context.Items.ToListAsync();
        }
    }
}