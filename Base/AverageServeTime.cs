using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class AverageServeTime : IAverageServeTime
    {
        private readonly TodoContext _context;

        public AverageServeTime(TodoContext context)
        {
            _context = context;
        }

        public float Get()
        {
            throw new System.NotImplementedException();
        }
    }
}