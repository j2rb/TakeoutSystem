using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class CanceledOrdersPercentage : ICanceledOrdersPercentage
    {
        private readonly TodoContext _context;

        public CanceledOrdersPercentage(TodoContext context)
        {
            _context = context;
        }

        public float Get()
        {
            throw new System.NotImplementedException();
        }
    }
}