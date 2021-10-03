using System;
using System.Linq;
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

        public decimal Get()
        {
            try
            {
                return (decimal)_context.Orders.Count(o => o.Status == 0) / _context.Orders.Count() * 100;
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }
    }
}