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
            throw new System.NotImplementedException();
        }
    }
}