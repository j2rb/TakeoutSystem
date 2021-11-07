using System;
using System.Threading.Tasks;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderReport
    {
        public Task<ReportFileDTO> GetReport(DateTime startDate, DateTime endDate);
    }
}
