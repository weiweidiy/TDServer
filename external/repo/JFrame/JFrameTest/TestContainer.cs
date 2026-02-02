using JFramework;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JFrameTest
{
    internal class TestContainer
    {
        private DictionaryContainer<TestItem> _container;
        private Action<ICollection<TestItem>> _itemAddedHandler;
        private Action<TestItem> _itemRemovedHandler;
        private Action<TestItem> _itemUpdatedHandler;

        private class TestItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        [SetUp]
        public void Setup()
        {
            _container = new DictionaryContainer<TestItem>(item => item.Id);

            // 使用 NSubstitute 创建 mock 事件处理器
            _itemAddedHandler = Substitute.For<Action<ICollection<TestItem>>>();
            _itemRemovedHandler = Substitute.For<Action<TestItem>>();
            _itemUpdatedHandler = Substitute.For<Action<TestItem>>();

            _container.onItemAdded += _itemAddedHandler;
            _container.onItemRemoved += _itemRemovedHandler;
            _container.onItemUpdated += _itemUpdatedHandler;
        }

        [TearDown]
        public void Teardown()
        {
            _container.onItemAdded -= _itemAddedHandler;
            _container.onItemRemoved -= _itemRemovedHandler;
            _container.onItemUpdated -= _itemUpdatedHandler;
        }

        [Test]
        public void Add_ShouldAddItemAndTriggerEvent()
        {
            // Arrange
            var item = new TestItem { Id = "1", Name = "Test" };

            // Act
            _container.Add(item);

            // Assert
            var result = _container.Get("1");
            Assert.AreEqual(item, result);
            _itemAddedHandler.Received(1).Invoke(Arg.Is<ICollection<TestItem>>(x => x.Contains(item)));
        }

        [Test]
        public void Add_WithNullItem_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _container.Add(null));
        }

        [Test]
        public void Add_WithNullKey_ShouldThrow()
        {
            var item = new TestItem { Id = null, Name = "Test" };
            Assert.Throws<ArgumentException>(() => _container.Add(item));
        }

        [Test]
        public void AddRange_ShouldAddAllItemsAndTriggerSingleEvent()
        {
            // Arrange
            var items = new List<TestItem>
        {
            new TestItem { Id = "1", Name = "Test1" },
            new TestItem { Id = "2", Name = "Test2" }
        };

            // Act
            _container.AddRange(items);

            // Assert
            Assert.AreEqual(2, _container.Count());
            _itemAddedHandler.Received(1).Invoke(Arg.Is<ICollection<TestItem>>(x => x.Count == 2));
        }

        [Test]
        public void Get_WithExistingKey_ShouldReturnItem()
        {
            // Arrange
            var item = new TestItem { Id = "1", Name = "Test" };
            _container.Add(item);

            // Act
            var result = _container.Get("1");

            // Assert
            Assert.AreEqual(item, result);
        }

        [Test]
        public void Get_WithNonExistingKey_ShouldThrow()
        {
            Assert.IsNull(_container.Get("999"));
        }

        [Test]
        public void TryGet_WithExistingKey_ShouldReturnTrueAndItem()
        {
            // Arrange
            var item = new TestItem { Id = "1", Name = "Test" };
            _container.Add(item);

            // Act
            var success = _container.TryGet("1", out var result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(item, result);
        }

        [Test]
        public void TryGet_WithNonExistingKey_ShouldReturnFalse()
        {
            var success = _container.TryGet("999", out var result);
            Assert.IsFalse(success);
            Assert.IsNull(result);
        }

        [Test]
        public void Remove_WithExistingKey_ShouldRemoveItemAndTriggerEvent()
        {
            // Arrange
            var item = new TestItem { Id = "1", Name = "Test" };
            _container.Add(item);

            // Act
            var removed = _container.Remove("1");

            // Assert
            Assert.IsTrue(removed);
            Assert.AreEqual(0, _container.Count());
            _itemRemovedHandler.Received(1).Invoke(item);
        }

        [Test]
        public void Remove_WithNonExistingKey_ShouldReturnFalse()
        {
            var removed = _container.Remove("999");
            Assert.IsFalse(removed);
            _itemRemovedHandler.DidNotReceiveWithAnyArgs().Invoke(default);
        }

        [Test]
        public void Update_ShouldUpdateItemAndTriggerEvent()
        {
            // Arrange
            var originalItem = new TestItem { Id = "1", Name = "Original" };
            _container.Add(originalItem);

            var updatedItem = new TestItem { Id = "1", Name = "Updated" };

            // Act
            _container.Update(updatedItem);

            // Assert
            var result = _container.Get("1");
            Assert.AreEqual("Updated", result.Name);
            _itemUpdatedHandler.Received(1).Invoke(updatedItem);
        }

        [Test]
        public void Update_WithNonExistingKey_ShouldThrow()
        {
            var item = new TestItem { Id = "1", Name = "Test" };
            Assert.Throws<KeyNotFoundException>(() => _container.Update(item));
        }

        [Test]
        public void Clear_ShouldRemoveAllItemsAndTriggerEvents()
        {
            // Arrange
            var items = new List<TestItem>
        {
            new TestItem { Id = "1", Name = "Test1" },
            new TestItem { Id = "2", Name = "Test2" }
        };
            _container.AddRange(items);

            // Act
            _container.Clear();

            // Assert
            Assert.AreEqual(0, _container.Count());
            _itemRemovedHandler.Received(1).Invoke(items[0]);
            _itemRemovedHandler.Received(1).Invoke(items[1]);
        }

        [Test]
        public void GetAll_ShouldReturnAllItems()
        {
            // Arrange
            var items = new List<TestItem>
        {
            new TestItem { Id = "1", Name = "Test1" },
            new TestItem { Id = "2", Name = "Test2" }
        };
            _container.AddRange(items);

            // Act
            var result = _container.GetAll();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.Contains(items[0], result);
            Assert.Contains(items[1], result);
        }

        [Test]
        public void Get_WithPredicate_ShouldReturnFilteredItems()
        {
            // Arrange
            var items = new List<TestItem>
        {
            new TestItem { Id = "1", Name = "Apple" },
            new TestItem { Id = "2", Name = "Banana" },
            new TestItem { Id = "3", Name = "Apple" }
        };
            _container.AddRange(items);

            // Act
            var result = _container.Get(item => item.Name == "Apple");

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.True(result.TrueForAll(x => x.Name == "Apple"));
        }
    }
}
