using System;
using AutoMapper;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Mappers
{
    public class OrderSimpleMapper : Profile
    {
        public OrderSimpleMapper()
        {
            CreateMap<Order, OrderSimpleDTO>().ReverseMap();
        }
    }
}
