using System;
using System.Collections.Generic;
using System.Linq;
using TakeoutSystem.Interfaces;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class ItemService : IItemService
    {
        private readonly TodoContext _context;

        public ItemService(TodoContext context)
        {
            _context = context;
        }

        public List<ItemDTO> GetItems()
        {
            return _context.Items.Select(i => new ItemDTO {
                ItemId = i.ItemId,
                Name = i.Name,
                Price = i.Price
            }).ToList();
        }
    }
}
