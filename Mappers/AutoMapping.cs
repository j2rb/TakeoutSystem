using System;
using AutoMapper;
using TakeoutSystem.DTO;

namespace TakeoutSystem.Mappers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<OrderDTO, OrderDetailsDTO>();
            CreateMap<OrderDTO, OrderSimpleDTO>();
        }
    }
}