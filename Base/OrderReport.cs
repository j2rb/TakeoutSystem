using System;
using System.Collections.Generic;
using OfficeOpenXml;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderReportExcel : IOrderReport
    {
        private readonly TodoContext _context;

        public OrderReportExcel(TodoContext context)
        {
            _context = context;
        }

        public byte[] Get(DateTime startDate, DateTime endDate)
        {
            IOrderReportData orderReportData = new OrderReportData(_context);
            List<OrderSimpleDTO> orders = orderReportData.Get(startDate, endDate);
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Orders");
                worksheet.Cells["A1"].Value = "Order Code";
                worksheet.Cells["B1"].Value = "Client Name";
                worksheet.Cells["C1"].Value = "Total";
                worksheet.Cells["A1:C1"].Style.Font.Bold = true;
                for (var i = 0; i < orders.Count; i++)
                {
                    worksheet.Cells["A" + (i + 2)].Value = orders[i].OrderCode;
                    worksheet.Cells["B" + (i + 2)].Value = orders[i].ClientName;
                    worksheet.Cells["C" + (i + 2)].Value = orders[i].Total;
                }
                return package.GetAsByteArray();
            }
        }
    }
}
