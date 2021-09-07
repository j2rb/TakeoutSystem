using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TakeoutSystem.Controllers
{
    [Route("/Home")]
    public class ValuesController : Controller
    {
        // GET: /Home
        [HttpGet]
        public String Get()
        {
            return "TakeoutSystem API";
        }
    }
}
