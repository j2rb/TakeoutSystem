using System;
namespace TakeoutSystem.DTO
{
    public class ItemOrderDTO : ItemDTO
    {
        public int Quantity { get; set; }
        public Decimal Total { get; set; }
    }
}
