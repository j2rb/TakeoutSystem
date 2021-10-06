using System;
namespace TakeoutSystem.Models
{
    public class OrderRequest
    {
            public int? page { get; set; }
            public int? pageSize { get; set; }
            public bool? onlyPending { get; set; }
    }
}
