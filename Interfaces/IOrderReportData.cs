using System;
using System.Collections.Generic;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Interfaces
{
    public interface IOrderReportData
    {
        public List<OrderSimpleDTO> Get(DateTime startDate, DateTime endDate);
    }
}
