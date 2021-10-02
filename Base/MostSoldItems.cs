using System;
using System.Collections.Generic;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class MostSoldItems : IMostSoldItems
    {
        private readonly TodoContext _context;

        public MostSoldItems(TodoContext context)
        {
            _context = context;
        }

        public List<ItemSimpleDTO> Get()
        {
            throw new NotImplementedException();
        }
    }
}
