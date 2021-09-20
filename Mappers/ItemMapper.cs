using System;
using AutoMapper;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Mappers
{
    public class ItemMapper : Profile
    {
        public ItemMapper()
        {
            CreateMap<Item, ItemDTO>().ReverseMap();
        }
    }
}
