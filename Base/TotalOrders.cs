using System.Linq;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class TotalOrders : ITotalOrders
    {
        private readonly TodoContext _context;

        public TotalOrders(TodoContext context)
        {
            _context = context;
        }

        public int Get()
        {
            return _context.Orders
               .Count(o => o.Status == 1);
        }
    }
}