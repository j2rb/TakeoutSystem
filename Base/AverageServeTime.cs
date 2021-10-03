using System;
using System.Linq;
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

        public decimal Get()
        {
            try
            {
                return (decimal)_context.Orders
                    .Where(o => o.Status == 1 && o.ServedAt != null)
                    .ToList()
                    .Sum(o => (o.ServedAt.GetValueOrDefault() - o.CreatedAt).TotalSeconds)                  
                    / _context.Orders.Where(o => o.Status == 1 && o.ServedAt != null).Count();
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }
    }
}