using System;
using System.Threading.Tasks;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderReport
    {
        public Task<ReportFileDTO> GetReportAsync(ReportRequest reportRequest);
    }
}
