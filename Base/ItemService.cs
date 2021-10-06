using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using TakeoutSystem.DTO;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class ItemService : IItemService
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public ItemService(TodoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<ItemDTO> GetItems()
        {
            return _context.Items.ProjectTo<ItemDTO>(_mapper.ConfigurationProvider).ToList();
        }
    }
}
