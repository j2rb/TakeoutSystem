using System;
using System.Threading.Tasks;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderReport
    {
        public Task<ReportFileDTO> GetReportAsync(DateTime startDate, DateTime endDate);
    }
}
