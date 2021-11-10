using System;
namespace TakeoutSystem.Models
{
    public class OrderRequest
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public bool? OnlyPending { get; set; }
        public int? Status { get; set; }
        public bool? Served { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public String OrderCode { get; set; }
    }
}
