using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;
using TakeoutSystem.Base;

namespace TakeoutSystem.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public MenuItemController(TodoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: MenuItem
        [Route("/MenuItem")]
        [HttpGet]
        public  List<ItemDTO> GetItems()
        {
            ListItems listItems = new ListItems(_context, _mapper);
            return listItems.GetList();
        }
    }
}