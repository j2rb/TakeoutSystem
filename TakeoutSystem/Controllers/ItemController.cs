using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<List<ItemDTO>> GetItemsAsync()
        {
           return await _itemService.GetItems();
        }
    }
}