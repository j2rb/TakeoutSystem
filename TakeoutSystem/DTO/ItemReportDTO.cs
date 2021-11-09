using System;
namespace TakeoutSystem.DTO
{
    public class ItemReportDTO : ItemDTO
    {
        public int Total { get; set; }
        public String TotalSum { get; set; }
        public String ShareInTotalIncome { get; set; }
    }
}
