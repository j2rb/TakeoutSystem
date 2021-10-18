using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TakeoutSystem.Base;
using TakeoutSystem.DTO;
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
        public void GetItemTest()
        {
            IItemService itemService = new ItemService(_context);
            var items = itemService.GetItems();

            Assert.That(items.Count() == 3);
            CollectionAssert.AllItemsAreUnique(items);
        }
    }
}

            /*
            var itemService = new Mock<IItemService>();
            var items = new List<ItemDTO>();
            items.Add(new ItemDTO { ItemId = 1, Name = "Soda", Price = 2.4m });
            items.Add(new ItemDTO { ItemId = 2, Name = "Hamburger", Price = 4.9m });
            items.Add(new ItemDTO { ItemId = 3, Name = "Hot Dog", Price = 2.65m });

            itemService.Setup(i => i.GetItems()).Returns(items);

            var result = itemService.Object.GetItems();
            */
            /*
            IItemService itemService = new ItemService(_context);
            var items = itemService.GetItems();

            Assert.That(items.Count() == 3);
            CollectionAssert.AllItemsAreInstancesOfType(items, typeof(ItemDTO));
            Assert.IsInstanceOf(typeof(int), items[0].ItemId);
            Assert.IsInstanceOf(typeof(String), items[0].Name);
            Assert.IsInstanceOf(typeof(Decimal), items[0].Price);
            CollectionAssert.AllItemsAreUnique(items);
            */
        