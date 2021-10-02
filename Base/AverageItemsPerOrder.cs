using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class AverageItemsPerOrder : IAverageItemsPerOrder
    {
        private readonly TodoContext _context;

        public AverageItemsPerOrder(TodoContext context)
        {
            _context = context;
        }

        public float Get()
        {
            throw new System.NotImplementedException();
        }
    }
}