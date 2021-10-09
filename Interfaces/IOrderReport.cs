using System;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderReport
    {
        public byte[] GetReport(DateTime startDate, DateTime endDate);
    }
}
