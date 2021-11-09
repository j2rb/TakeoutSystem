using System;
namespace TakeoutSystem.DTO
{
    public class OrderReportDTO
    {
        public String Created { get; set; }
        public int ItemCount { get; set; }
        public String TotalAmount { get; set; }
        public String Finished { get; set; }
    }
}
