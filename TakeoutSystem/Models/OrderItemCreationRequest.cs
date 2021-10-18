using System;
namespace TakeoutSystem.Models
{
    public class OrderItemCreationRequest
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
