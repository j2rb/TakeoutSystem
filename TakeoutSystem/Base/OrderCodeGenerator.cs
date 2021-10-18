using System;
using TakeoutSystem.Interfaces;

namespace TakeoutSystem.Base
{
    public class OrderCodeGenerator : IOrderCodeGenerator
    {
        public string GetCode()
        {
            Guid orderCode = Guid.NewGuid();
            return orderCode.ToString();
        }
    }
}
