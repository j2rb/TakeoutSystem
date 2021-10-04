﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
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
        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public ReportController(TodoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Reports
        [Route("/Reports/Statistics")]
        [HttpGet]
        public ActionResult<OrderStatisticsDTO> Get()
        {
            try
            {
                IOrderStatistics orderStatistics = new OrderStatisticts(_context);
                return orderStatistics.Get();
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
            IOrderReport orderReport = new OrderReportExcel(_context);
            return File(orderReport.Get(startDate, endDate), contentType, fileName);
        }
    }
}
