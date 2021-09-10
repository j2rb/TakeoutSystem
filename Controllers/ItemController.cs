using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;


namespace TakeoutSystem.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly TodoContext _context;
        private MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.CreateMap<Item, ItemDTO>());

        public MenuItemController(TodoContext context)
        {
            _context = context;
        }

        // GET: MenuItem
        [Route("/MenuItem")]
        [HttpGet]
        public  List<ItemDTO> GetItems()
        {
            return _context.Items.ProjectTo<ItemDTO>(configuration).ToList();
        }
    }
}