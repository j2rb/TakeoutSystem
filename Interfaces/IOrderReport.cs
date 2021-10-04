using System;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderReport
    {
        public byte[] Get(DateTime startDate, DateTime endDate);
    }
}
