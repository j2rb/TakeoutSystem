using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using TakeoutSystem.DTO;
using TakeoutSystem.Models;

namespace TakeoutSystem.Base
{
    public class ListOrders
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public ListOrders(TodoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<OrderSimpleDTO> GetList(Int16? Page, Int16? PageSize, Boolean? OnlyPending)
        {
            //Default values
            Page = Page == null || Page <= 0 ? 1 : Page;
            PageSize = PageSize == null || PageSize <= 0 ? 10 : PageSize;
            OnlyPending = OnlyPending == null ? true : OnlyPending;
            //Get orders
            return (
                    _context.Order
                    .Join(
                        _context.OrderItem, o => o.OrderId, oi => oi.OrderId, (order, orderItem) => new { order, orderItem }
                    )
                    .Where(o => (OnlyPending.GetValueOrDefault() == true ? o.order.ServedAt == null : true) && o.order.Status == 1)
                    .GroupBy(o => new { o.order.OrderCode, o.order.ClientName, o.order.CreatedAt })
                    .OrderBy(o => o.Key.CreatedAt)
                    .Select(o => new OrderSimpleDTO
                    {
                        OrderCode = o.Key.OrderCode,
                        ClientName = o.Key.ClientName,
                        Total = o.Count()
                    })
                )
                .Skip(Page.GetValueOrDefault() * PageSize.GetValueOrDefault() - PageSize.GetValueOrDefault())
                .Take(PageSize.GetValueOrDefault())
                .ToList();
        }
    }
}
