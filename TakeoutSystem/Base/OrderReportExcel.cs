using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class OrderReportExcel : IOrderReport
    {
        private readonly IOrderStatistics _orderStatistics;
        private readonly IOrderService _orderService;

        public OrderReportExcel(IOrderStatistics orderStatistics, IOrderService orderService)
        {
            _orderStatistics = orderStatistics;
            _orderService = orderService;
        }

        public async Task<ReportFileDTO> GetReportAsync(DateTime startDate, DateTime endDate)
        {
            var report = new ReportFileDTO
            {
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = "TakeoutReport_" + startDate.ToString("yyyy-MM-dd") + "_to_" + endDate.ToString("yyyy-MM-dd") + ".xlsx",
            };
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Order Statistics");
                int row = 1, column = 1;
                worksheet.Cells[row, column].Value = "Takeout system report";

                row += 1;
                worksheet.Cells[row, column++].Value = "For period from";
                worksheet.Cells[row, column++].Value = startDate.ToString("MM/dd/yyyy");
                worksheet.Cells[row++, column++].Value = endDate.ToString("MM/dd/yyyy");

                var orderStatisticRequest = new OrderStatisticRequest
                {
                    StartDate = startDate,
                    EndDate = endDate
                };

                Dictionary<String, String> data = new Dictionary<String, String>();
                data.Add("Summary", "");
                data.Add("Total Orders", (await _orderStatistics.TotalCountAsync(orderStatisticRequest)).ToString());
                var totalPriceOrders = await _orderStatistics.TotalPriceOrdersAsync(orderStatisticRequest);
                data.Add("Total Sum", "$ " + totalPriceOrders);
                data.Add("Average Order Price", "$ " + await _orderStatistics.AveragePriceOrdersAsync(orderStatisticRequest));
                data.Add("Average Items Per Order", (await _orderStatistics.AverageItemsPerOrderAsync(orderStatisticRequest)).ToString());
                data.Add("Cancelled Orders", await _orderStatistics.CanceledOrdersCountAsync(orderStatisticRequest) + " (" + (await _orderStatistics.CanceledOrdersPercentageAsync(orderStatisticRequest)).ToString("0.00") + "%)");
                data.Add("Average Serve Time", (await _orderStatistics.AverageServeTimeAsync(orderStatisticRequest) / 60).ToString("0.00") + " minutes");

                row++;
                foreach (var item in data)
                {
                    column = 1;
                    worksheet.Cells[row, column++].Value = item.Key;
                    worksheet.Cells[row++, column++].Value = data[item.Key];
                }

                var orders = await _orderService.GetOrdersAsync(new OrderRequest
                {
                    StartDate = startDate,
                    EndDate = endDate
                });

                var orderItems = orders.SelectMany(o => o.Items);
                var items = orderItems.GroupBy(i => new { i.ItemId, i.Name, i.Price })
                    .Select(i => new
                    {
                        i.Key.ItemId,
                        i.Key.Name,
                        i.Key.Price,
                        Total = i.Sum(i => i.Quantity),
                        TotalSum = i.Sum(i => i.Quantity * i.Price).ToString("0.00"),
                        ShareInTotalIncome = (i.Sum(i => i.Quantity * i.Price) / totalPriceOrders * 100).ToString("0.00") + "%"
                    })
                    .ToList();

                row++;
                column = 1;
                worksheet.Cells[row++, column].Value = "Item Statistics";
                worksheet.Cells[row, column++].Value = "Id";
                worksheet.Cells[row, column++].Value = "Name";
                worksheet.Cells[row, column++].Value = "Price";
                worksheet.Cells[row, column++].Value = "Sold Quantity";
                worksheet.Cells[row, column++].Value = "Sold Total Sum";
                worksheet.Cells[row++, column].Value = "Share in Total Income";
                for (var i = 0; i < items.Count; i++)
                {
                    column = 1;
                    worksheet.Cells[row, column++].Value = items[i].ItemId;
                    worksheet.Cells[row, column++].Value = items[i].Name;
                    worksheet.Cells[row, column++].Value = items[i].Price;
                    worksheet.Cells[row, column++].Value = items[i].Total;
                    worksheet.Cells[row, column++].Value = items[i].TotalSum;
                    worksheet.Cells[row++, column].Value = items[i].ShareInTotalIncome;
                }

                row++;
                column = 1;
                worksheet.Cells[row++, column].Value = "Order Statistics";
                worksheet.Cells[row, column++].Value = "Created";
                worksheet.Cells[row, column++].Value = "Item Count";
                worksheet.Cells[row, column++].Value = "Total Amount";
                worksheet.Cells[row++, column].Value = "Finished";
                for (var i = 0; i < orders.Count; i++)
                {
                    column = 1;
                    worksheet.Cells[row, column++].Value = orders[i].CreatedAt.ToString("dd/MM/yy HH:mm");
                    worksheet.Cells[row, column++].Value = orders[i].Items.Sum(i => i.Quantity);
                    worksheet.Cells[row, column++].Value = "$ " + orders[i].Items.Sum(i => i.Price * i.Quantity).ToString("0.00");
                    worksheet.Cells[row++, column].Value = orders[i].ServedAt == null ? "" : orders[i].ServedAt.GetValueOrDefault().ToString("dd/MM/yy HH:mm");
                }
                report.Data = package.GetAsByteArray();
                return report;
            }
        }
    }
}
