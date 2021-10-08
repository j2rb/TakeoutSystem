using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TakeoutSystem.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IOrderStatistics _orderStatisticts; 

        public ReportController(IOrderStatistics orderStatisticts)
        {
            _orderStatisticts = orderStatisticts;
        }

        // GET: Reports
        [Route("/Reports/Statistics")]
        [HttpGet]
        public ActionResult<OrderStatisticsDTO> Get()
        {
            try
            {
                return new OrderStatisticsDTO {
                    MostSoldItems = _orderStatisticts.GetMostSoldItems(),
                    AverageServeTimeInSeconds = _orderStatisticts.GetAverageServeTime(),
                    AverageItemsPerOrder = _orderStatisticts.GetAverageItemsPerOrder(),
                    CanceledOrdersPercentage = _orderStatisticts.CanceledOrdersPercentage(),
                    TotalOrders = _orderStatisticts.GetCount()
                };
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // GET: Orders
        /*[Route("/Reports/Orders")]
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
            IOrderReport orderReport = new OrderReportExcel(_context);
            return File(orderReport.Get(startDate, endDate), contentType, fileName);
        }*/
    }
}
