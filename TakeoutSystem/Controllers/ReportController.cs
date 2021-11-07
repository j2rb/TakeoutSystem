using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TakeoutSystem.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IOrderStatistics _orderStatisticts;
        private readonly IOrderReport _orderReport;

        public ReportController(IOrderStatistics orderStatisticts, IOrderReport orderReport)
        {
            _orderStatisticts = orderStatisticts;
            _orderReport = orderReport;
        }

        // GET: Reports
        [Route("/Reports/Statistics")]
        [HttpGet]
        public async Task<ActionResult<OrderStatisticsDTO>> Get()
        {
            var orderStatisticRequest = new OrderStatisticRequest { };
            return new OrderStatisticsDTO
            {
                MostSoldItems = await _orderStatisticts.MostSoldItems(orderStatisticRequest),
                AverageServeTimeInSeconds = await _orderStatisticts.AverageServeTime(orderStatisticRequest),
                AverageItemsPerOrder = await _orderStatisticts.AverageItemsPerOrder(orderStatisticRequest),
                CanceledOrdersPercentage = await _orderStatisticts.CanceledOrdersPercentage(orderStatisticRequest),
                TotalOrders = await _orderStatisticts.TotalCount(orderStatisticRequest)
            };
        }

        // GET: Orders
        [Route("/Reports/Orders")]
        [HttpGet]
        public async Task<IActionResult> GetExcelReport(DateTime StartDate, DateTime EndDate)
        {
            var report = await _orderReport.GetReport(StartDate, EndDate);
            return File(report.Data, report.ContentType, report.FileName);
        }
    }
}
