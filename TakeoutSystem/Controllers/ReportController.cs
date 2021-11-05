using System;
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
        public ActionResult<OrderStatisticsDTO> Get()
        {
            return new OrderStatisticsDTO
            {
                MostSoldItems = _orderStatisticts.MostSoldItems(new OrderStatisticRequest { }),
                AverageServeTimeInSeconds = Math.Round(_orderStatisticts.AverageServeTime(new OrderStatisticRequest { }), 2),
                AverageItemsPerOrder = Math.Round(_orderStatisticts.AverageItemsPerOrder(new OrderStatisticRequest { }), 2),
                CanceledOrdersPercentage = Math.Round(_orderStatisticts.CanceledOrdersPercentage(new OrderStatisticRequest { }), 2),
                TotalOrders = _orderStatisticts.TotalCount(new OrderStatisticRequest { })
            };
        }

        // GET: Orders
        [Route("/Reports/Orders")]
        [HttpGet]
        public IActionResult GetExcelReport(String StartDate, String EndDate)
        {
            DateTime startDate;
            DateTime endDate;
            if (!DateTime.TryParse(StartDate, out startDate))
            {
                return Problem(title: "Wrong argument in request", detail: "Invalid start date", statusCode: 400);
            }
            else if (!DateTime.TryParse(EndDate, out endDate))
            {
                return Problem(title: "Wrong argument in request", detail: "Invalid end date", statusCode: 400);
            }
            else if (startDate >= endDate)
            {
                return Problem(title: "Wrong argument in request", detail: "Start date must be greater than end date", statusCode: 400);
            }
            var report = _orderReport.GetReport(startDate, endDate);
            return File(report.Data, report.ContentType, report.FileName);
        }
    }
}
