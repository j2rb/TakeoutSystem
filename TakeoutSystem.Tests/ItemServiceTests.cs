using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TakeoutSystem.Base;
using TakeoutSystem.Interfaces;
using TakeoutSystem.Models;

namespace TakeoutSystem.Tests
{
    public class ItemServiceTests
    {
        private TodoContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase("TakeoutSystem").Options;
            _context = new TodoContext(options);
            _context.Items.Add(new Item
            {
                ItemId = 1,
                Name = "ITEM-1",
                Price = 9.99m
            });
            _context.Items.Add(new Item
            {
                ItemId = 2,
                Name = "ITEM-2",
                Price = 5.5m
            });
            _context.Items.Add(new Item
            {
                ItemId = 3,
                Name = "ITEM-3",
                Price = 2.65m
            });
            _context.SaveChanges();
        }

        [Test]
        public async Task GetItemTest()
        {
            IItemService itemService = new ItemService(_context);
            var items = await itemService.GetItems();

            Assert.That(items.Count() == 3);
            CollectionAssert.AllItemsAreUnique(items);
        }
    }
}