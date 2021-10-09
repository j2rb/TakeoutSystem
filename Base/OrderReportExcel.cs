using System;
using System.Linq;
using OfficeOpenXml;
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

        public byte[] GetReport(DateTime startDate, DateTime endDate)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Orders");
                worksheet.Cells["A1"].Value = "Takeout system report";
                worksheet.Cells["A2"].Value = "For period from";
                worksheet.Cells["B2"].Value = startDate.ToString("MM/dd/yyyy");
                worksheet.Cells["C2"].Value = endDate.ToString("MM/dd/yyyy");

                var orderStatisticRequest = new OrderStatisticRequest {
                    StartDate = startDate,
                    EndDate = endDate
                };

                worksheet.Cells["A4"].Value = "Summary";
                worksheet.Cells["A4"].Style.Font.Bold = true;
                worksheet.Cells["A5"].Value = "Total Orders";
                worksheet.Cells["D5"].Value = _orderStatistics.TotalCount(orderStatisticRequest);

                worksheet.Cells["A6"].Value = "Total Sum";
                var totalPriceOrders = _orderStatistics.TotalPriceOrders(orderStatisticRequest);
                worksheet.Cells["D6"].Value = "$ " + totalPriceOrders.ToString("0.00");

                worksheet.Cells["A7"].Value = "Average Order Price";
                worksheet.Cells["D7"].Value = "$ " + _orderStatistics.AveragePriceOrders(orderStatisticRequest).ToString("0.00");

                worksheet.Cells["A8"].Value = "Average Items Per Order";
                worksheet.Cells["D8"].Style.Numberformat.Format = "0.00";
                worksheet.Cells["D8"].Value = _orderStatistics.AverageItemsPerOrder(orderStatisticRequest);

                worksheet.Cells["A9"].Value = "Cancelled Orders";
                worksheet.Cells["D9"].Value = _orderStatistics.CanceledOrdersCount(orderStatisticRequest) + " ("  + _orderStatistics.CanceledOrdersPercentage(orderStatisticRequest).ToString("0.00") + "%)";

                worksheet.Cells["A10"].Value = "Average Serve Time";
                worksheet.Cells["D10"].Value = (_orderStatistics.AverageServeTime(orderStatisticRequest) / 60).ToString("0.00") + " minutes";

                var orders = _orderService.GetOrders(new OrderRequest
                {
                    Status = 1,
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
                        TotalSum = i.Sum(i => i.Quantity * i.Price),
                        ShareInTotalIncome = i.Sum(i => i.Quantity * i.Price) / totalPriceOrders * 100 
                    })
                    .ToList(); 
                worksheet.Cells["A12"].Value = "Item Statistics";
                worksheet.Cells["A12"].Style.Font.Bold = true;
                worksheet.Cells["A13"].Value = "Id";
                worksheet.Cells["B13"].Value = "Name";
                worksheet.Cells["C13"].Value = "Price";
                worksheet.Cells["D13"].Value = "Sold Quantity";
                worksheet.Cells["E13"].Value = "Sold Total Sum";
                worksheet.Cells["F13"].Value = "Share in Total Income";
                int position = 14;
                for (var i = 0; i < items.Count; i++)
                {
                    worksheet.Cells["A" + position].Value = items[i].ItemId;
                    worksheet.Cells["B" + position].Value = items[i].Name;
                    worksheet.Cells["C" + position].Value = items[i].Price;
                    worksheet.Cells["D" + position].Value = items[i].Total;
                    worksheet.Cells["E" + position].Value = items[i].TotalSum.ToString("0.00");
                    worksheet.Cells["F" + position++].Value = items[i].ShareInTotalIncome.ToString("0.00") + "%";
                }

                position += 2;
                worksheet.Cells["A" + position].Value = "Order Statistics";
                worksheet.Cells["A" + position++].Style.Font.Bold = true;
                worksheet.Cells["A" + position].Value = "Created";
                worksheet.Cells["B" + position].Value = "Item Count";
                worksheet.Cells["C" + position].Value = "Total Amount";
                worksheet.Cells["D" + position++].Value = "Finished";
                for (var i = 0; i < orders.Count; i++)
                {
                    worksheet.Cells["A" + position].Value = orders[i].CreatedAt.ToString("dd/MM/yy HH:mm");
                    worksheet.Cells["B" + position].Value = orders[i].Items.Sum(i => i.Quantity);
                    worksheet.Cells["C" + position].Value = "$ " + orders[i].Items.Sum(i => i.Price * i.Quantity).ToString("0.00");
                    worksheet.Cells["D" + position++].Value = orders[i].ServedAt == null ? "" : orders[i].ServedAt.GetValueOrDefault().ToString("dd/MM/yy HH:mm");
                }
                return package.GetAsByteArray();
            }
        }
    }
}
