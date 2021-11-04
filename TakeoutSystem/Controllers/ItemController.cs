using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;

namespace TakeoutSystem.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        
        public MenuItemController(IItemService itemService)
        {
            _itemService = itemService;
        }
        // GET: MenuItem
        [Route("/MenuItem")]
        [HttpGet]
        public ActionResult<List<ItemDTO>> GetItems()
        {
           return _itemService.GetItems();
        }
    }
}