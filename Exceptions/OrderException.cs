using System;

namespace TakeoutSystem.Exceptions
{
    public class OrderException : Exception
    {
        public OrderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}