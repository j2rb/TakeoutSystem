﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class ListItems
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public ListItems(TodoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<ItemDTO> GetList()
        {
            return _context.Items.ProjectTo<ItemDTO>(_mapper.ConfigurationProvider).ToList();
        }
    }
}
