using System;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderReport
    {
        public ReportFileDTO GetReport(DateTime startDate, DateTime endDate);
    }
}
