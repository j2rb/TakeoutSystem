using System;
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
                if (items.Count > 0)
                {
                    var itemProperties = items[0].GetType().GetProperties();
                    for (var i = 0; i < items.Count; i++)
                    {
                        for (var j = 0; j < itemProperties.Count(); j++)
                        {
                            if (i == 0)
                            {
                                worksheet.Cells[row + i, column + j].Value = itemProperties[j].Name;
                            }
                            worksheet.Cells[row + i + 1, column + j].Value = items[i].GetType().GetProperty(itemProperties[j].Name).GetValue(items[i], null);
                        }
                    }
                }
                row = row + items.Count + 2;

                worksheet.Cells[row++, column].Value = "Order Statistics";
                var ordersData = orders.Select(o => new
                {
                    Created = o.CreatedAt.ToString("dd/MM/yy HH:mm"),
                    ItemCount = o.Items.Sum(i => i.Quantity),
                    TotalAmount = "$ " + o.Items.Sum(i => i.Price * i.Quantity).ToString("0.00"),
                    Finished = o.ServedAt == null ? "" : o.ServedAt.GetValueOrDefault().ToString("dd/MM/yy HH:mm")
                }).ToList();
                if (ordersData.Count > 0)
                {
                    var orderProperties = ordersData[0].GetType().GetProperties();
                    for (var i = 0; i < ordersData.Count; i++)
                    {
                        for (var j = 0; j < orderProperties.Count(); j++)
                        {
                            if (i == 0)
                            {
                                worksheet.Cells[row + i, column + j].Value = orderProperties[j].Name;
                            }
                            worksheet.Cells[row + i + 1, column + j].Value = ordersData[i].GetType().GetProperty(orderProperties[j].Name).GetValue(ordersData[i], null);
                        }
                    }
                }
                report.Data = package.GetAsByteArray();
                return report;
            }
        }
    }
}
