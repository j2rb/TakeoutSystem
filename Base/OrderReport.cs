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
                worksheet.Cells["A1"].Value = "Takeout system report";
                worksheet.Cells["A2"].Value = "For period from";
                worksheet.Cells["B2"].Value = startDate.ToString("MM/dd/yyyy");
                worksheet.Cells["C2"].Value = endDate.ToString("MM/dd/yyyy");

                worksheet.Cells["A4"].Value = "Summary";
                worksheet.Cells["A4"].Style.Font.Bold = true;
                worksheet.Cells["A5"].Value = "Total Orders";
                worksheet.Cells["A6"].Value = "Total Sum";
                worksheet.Cells["A7"].Value = "Average Order Price";
                worksheet.Cells["A8"].Value = "Average Items Per Order";
                worksheet.Cells["A9"].Value = "Cancelled Orders";
                worksheet.Cells["A10"].Value = "Average Serve Time";
                worksheet.Cells["D5"].Value = "";
                worksheet.Cells["D6"].Value = "";
                worksheet.Cells["D7"].Value = "";
                worksheet.Cells["D8"].Value = "";
                worksheet.Cells["D9"].Value = "";
                worksheet.Cells["D10"].Value = "";

                worksheet.Cells["A12"].Value = "Item Statistics";
                worksheet.Cells["A12"].Style.Font.Bold = true;
                worksheet.Cells["A13"].Value = "Id";
                worksheet.Cells["B13"].Value = "Name";
                worksheet.Cells["C13"].Value = "Price";
                worksheet.Cells["D13"].Value = "Sold Quantity";
                worksheet.Cells["E13"].Value = "Sold Total Sum";
                worksheet.Cells["F13"].Value = "Share in Total Income";
                int position = 14;
                worksheet.Cells["A" + position].Value = "";
                worksheet.Cells["B" + position].Value = "";
                worksheet.Cells["C" + position].Value = "";
                worksheet.Cells["D" + position].Value = "";
                worksheet.Cells["E" + position].Value = "";
                worksheet.Cells["F" + position].Value = "";
                position++;
                position += 2;
                worksheet.Cells["A" + position].Value = "Order Statistics";
                worksheet.Cells["A" + position].Style.Font.Bold = true;
                worksheet.Cells["A" + position].Value = "Created";
                worksheet.Cells["B" + position].Value = "Item Count";
                worksheet.Cells["C" + position].Value = "Total Amount";
                worksheet.Cells["D" + position].Value = "Finished";
                position++;
                worksheet.Cells["A" + position].Value = "";
                worksheet.Cells["B" + position].Value = "";
                worksheet.Cells["C" + position].Value = "";
                worksheet.Cells["D" + position].Value = "";
                position++;
                /*for (var i = 0; i < orders.Count; i++)
                {
                    worksheet.Cells["A" + (i + 2)].Value = orders[i].OrderCode;
                    worksheet.Cells["B" + (i + 2)].Value = orders[i].ClientName;
                    worksheet.Cells["C" + (i + 2)].Value = orders[i].Total;
                }*/
                return package.GetAsByteArray();
            }
        }
    }
}
