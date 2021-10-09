using System;
using Microsoft.AspNetCore.Mvc;
using TakeoutSystem.Base;
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
            try
            {
                return new OrderStatisticsDTO {
                    MostSoldItems = _orderStatisticts.MostSoldItems(new OrderStatisticRequest { }),
                    AverageServeTimeInSeconds = _orderStatisticts.AverageServeTime(new OrderStatisticRequest { }),
                    AverageItemsPerOrder = _orderStatisticts.AverageItemsPerOrder(new OrderStatisticRequest { }),
                    CanceledOrdersPercentage = _orderStatisticts.CanceledOrdersPercentage(new OrderStatisticRequest { }),
                    TotalOrders = _orderStatisticts.TotalCount(new OrderStatisticRequest { })
                };
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // GET: Orders
        [Route("/Reports/Orders")]
        [HttpGet]
        public IActionResult GetExcelReport(String StartDate, String EndDate)
        {
            DateTime startDate;
            DateTime endDate;
            if (!DateTime.TryParse(StartDate, out startDate) || !DateTime.TryParse(EndDate, out endDate))
            {
                return BadRequest();
            }
            else if (startDate >= endDate)
            {
                return BadRequest();
            }
            String contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            String fileName = "TakeoutReport_" + startDate.ToString("yyyy-MM-dd") + "_to_" + endDate.ToString("yyyy-MM-dd") + ".xlsx";
            return File(_orderReport.GetReport(startDate, endDate), contentType, fileName);
        }
    }
}
