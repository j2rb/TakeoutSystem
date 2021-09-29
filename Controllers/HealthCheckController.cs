using System;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TakeoutSystem.Controllers
{
    [Route("/HealthCheck")]
    public class HealthCheck : Controller
    {
        // GET: /HealthCheck
        [HttpGet]
        public int Get()
        {
            return 1;
        }        
    }
}
