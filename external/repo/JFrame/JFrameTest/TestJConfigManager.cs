using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JFramework.Game;
using JFrameTest;
using System.Text;

namespace JFramework.Tests
{
    [TestFixture]
    public class JConfigManagerTests
    {
        // 测试模型
        public class TestItem : IUnique
        {
            public string Uid { get; set; }
            public int Value { get; set; }
        }

        // 模拟配置表
        //public class MockConfigTable : IConfigTable<TestItem>, IEnumerable<TestItem>
        //{
        //    private readonly List<TestItem> _items = new List<TestItem>();

        //    public void Initialize(TestItem[] lst) => _items.AddRange(lst);

        //    public IEnumerator<TestItem> GetEnumerator() => _items.GetEnumerator();

        //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //    //private static IEnumerable<TestItem> GetTestData() => new[]
        //    //{
        //    //    new TestItem { Uid = "item1", Value = 100 },
        //    //    new TestItem { Uid = "item2", Value = 200 }
        //    //};
        //}

        public class MockConfigTable:BaseConfigTable<TestItem>
        {

        }

        private JConfigManager _configManager;
        private IConfigLoader _mockLoader;
        JsonNetSerializer serializer = new JsonNetSerializer();

        List<TestItem> dataList = new List<TestItem>() {
                new TestItem() { Uid = "item1", Value = 100 },
                new TestItem() { Uid = "item2", Value = 200 }
            };

        byte[] bytesData;

        [SetUp]
        public void Setup()
        {
            _mockLoader = Substitute.For<IConfigLoader>();
            _configManager = new JConfigManager(_mockLoader);
            var json = serializer.Serialize(dataList);
            bytesData = Encoding.UTF8.GetBytes(json);
        }

        [Test]
        public async Task PreloadAllAsync_ShouldLoadAllRegisteredTables()
        {
            // Arrange
            _mockLoader.LoadBytesAsync(Arg.Any<string>()).Returns(bytesData);
            _configManager.RegisterTable<MockConfigTable, TestItem>("test_path", serializer);

            // Act
            await _configManager.PreloadAllAsync();

            // Assert
            await _mockLoader.Received(1).LoadBytesAsync("test_path");
            Assert.AreEqual(2, _configManager.GetAll<TestItem>().Count);
        }

        [Test]
        public void Get_ShouldReturnItem_WhenUidExists()
        {
            // Arrange
            var mockTable = Substitute.For<IConfigTable<TestItem>>();
            mockTable.GetEnumerator().Returns(new List<TestItem>
            {
                new TestItem { Uid = "item1", Value = 100 }
            }.GetEnumerator());

            // 使用反射注入模拟表
            typeof(JConfigManager)
                .GetField("_tables", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(_configManager, new Dictionary<Type, object> { [typeof(TestItem)] = mockTable });

            typeof(JConfigManager)
                .GetField("_uidMaps", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(_configManager, new Dictionary<Type, Dictionary<string, IUnique>>
                {
                    [typeof(TestItem)] = new Dictionary<string, IUnique> { ["item1"] = new TestItem { Uid = "item1", Value = 100 } }
                });

            // Act
            var result = _configManager.Get<TestItem>("item1");

            // Assert
            Assert.AreEqual(100, result.Value);
        }

        [Test]
        public void Get_ShouldThrow_WhenUidNotExists()
        {
            // Arrange
            typeof(JConfigManager)
                .GetField("_uidMaps", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(_configManager, new Dictionary<Type, Dictionary<string, IUnique>>());

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _configManager.Get<TestItem>("invalid_id"));
        }

        [Test]
        public void GetAll_ShouldReturnEmptyList_WhenTableNotLoaded()
        {
            // Act
            var result = _configManager.GetAll<TestItem>();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetData_ShouldCorect()
        {
            //arrange
            _mockLoader.LoadBytesAsync(Arg.Any<string>()).Returns(bytesData);
            _configManager.RegisterTable<MockConfigTable, TestItem>("test_path", serializer);

            // Act
            await _configManager.PreloadAllAsync();

            //assert
            Assert.AreEqual(2, _configManager.GetAll<TestItem>().Count);
            Assert.AreEqual(200, _configManager.Get<TestItem>("item2").Value);
        }

        [Test]
        public async Task GetDataList_ShouldCorect()
        {
            //arrange
            _mockLoader.LoadBytesAsync(Arg.Any<string>()).Returns(bytesData);
            _configManager.RegisterTable<MockConfigTable, TestItem>("test_path", serializer);

            // Act
            await _configManager.PreloadAllAsync();

            //assert
            Assert.AreEqual(2, _configManager.GetAll<TestItem>().Count);
            Assert.AreEqual("item2", _configManager.Get<TestItem>(i=>i.Value==200).First().Uid );
        }
    }
}