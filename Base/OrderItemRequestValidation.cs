using System;
using TakeoutSystem.Models;
using TakeoutSystem.Interfaces;
using System.Collections.Generic;
using TakeoutSystem.DTO;
using System.Linq;

namespace TakeoutSystem.Base
{
    public class OrderItemRequestValidation : IOrderItemRequestValidation
    {
        private readonly TodoContext _context;

        public OrderItemRequestValidation(TodoContext context)
        {
            _context = context;
        }

        public void Validate(List<ItemRequest> items)
        {
            if (items == null || items.Count == 0)
            {
                throw new ArgumentException();
            }
            else
            {
                IEnumerable<short> duplicates = items.GroupBy(i => i.ItemId).Where(i => i.Count() > 1).Select(i => i.Key);
                if (duplicates.Count() > 0)
                {
                    throw new ArgumentException();
                }
                for (var x = 0; x < items.Count; x++)
                {
                    if (items[x].ItemId <= 0)
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        Item item = _context.Items.SingleOrDefault(i => i.ItemId == items[x].ItemId);
                        if (item == null)
                        {
                            throw new ArgumentException();
                        }
                    }
                    if (items[x].Quantity <= 0)
                    {
                        throw new ArgumentException();
                    }
                }
            }
        }
    }
}
