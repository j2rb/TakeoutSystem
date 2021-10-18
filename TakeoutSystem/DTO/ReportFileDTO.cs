using System;
namespace TakeoutSystem.DTO
{
    public class ReportFileDTO
    {
        public String ContentType { get; set; }
        public String FileName { get; set; }
        public byte[] Data { get; set; }
    }
}
