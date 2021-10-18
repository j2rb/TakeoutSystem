using System;
using NUnit.Framework;
using TakeoutSystem.Base;
using TakeoutSystem.Interfaces;

namespace TakeoutSystem.Tests
{
    public class OrderCodeGeneratorTests
    {
        [Test]
        public void GenerateOrderCodeTest()
        {
            IOrderCodeGenerator orderCodeGenerator = new OrderCodeGenerator();
            var code = orderCodeGenerator.GetCode();

            Assert.IsInstanceOf(typeof(String), code);
            Assert.That(String.IsNullOrEmpty(code) == false);
        }
    }
}
