using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JFrameTest
{
    using NUnit.Framework;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::JFramework;
    using Newtonsoft.Json;


    [TestFixture]
    public class JDataStoreTests
    {
        private IDataManager _mockDataManager;
        private JDataStore _dataStore;

        [SetUp]
        public void Setup()
        {
            _mockDataManager = Substitute.For<IDataManager>();
            _dataStore = new JDataStore(_mockDataManager);
        }

        [TearDown]
        public void Teardown()
        {
            _dataStore.Dispose();
        }

        #region ExistsAsync Tests

        [Test]
        public async Task ExistsAsync_WhenKeyInMemory_ReturnsTrue()
        {
            // Arrange
            const string testKey = "testKey";
            var testValue = new TestData { Id = 1, Name = "Test" };
            await _dataStore.SaveAsync(testKey, testValue);

            // Act
            var result = await _dataStore.ExistsAsync(testKey);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ExistsAsync_WhenKeyInStorage_ReturnsTrue()
        {
            // Arrange
            const string testKey = "testKey";
            _mockDataManager.ExistsAsync(testKey).Returns(Task.FromResult(true));

            // Act
            var result = await _dataStore.ExistsAsync(testKey);

            // Assert
            Assert.IsTrue(result);
            await _mockDataManager.Received(1).ExistsAsync(testKey);
        }

        [Test]
        public async Task ExistsAsync_WhenKeyNotExists_ReturnsFalse()
        {
            // Arrange
            const string testKey = "nonExistentKey";
            _mockDataManager.ExistsAsync(testKey).Returns(Task.FromResult(false));

            // Act
            var result = await _dataStore.ExistsAsync(testKey);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ExistsAsync_WhenKeyIsNull_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => _dataStore.ExistsAsync(null));
        }

        #endregion

        #region GetAsync Tests

        [Test]
        public async Task GetAsync_WhenKeyInMemory_ReturnsCachedValue()
        {
            // Arrange
            const string testKey = "testKey";
            var expected = new TestData { Id = 1, Name = "Test" };
            await _dataStore.SaveAsync(testKey, expected);

            // Act
            var result = await _dataStore.GetAsync<TestData>(testKey);

            // Assert
            Assert.AreEqual(expected, result);
            _mockDataManager.DidNotReceive().ReadAsync<TestData>(Arg.Any<string>(), Arg.Any<IDataManager>());
        }

        [Test]
        public async Task GetAsync_WhenKeyInStorage_ReturnsValueFromStorage()
        {
            // Arrange
            const string testKey = "testKey";
            var expected = new TestData { Id = 1, Name = "Test" };
            _mockDataManager.ReadAsync<TestData>(testKey, _mockDataManager).Returns(Task.FromResult(expected));

            // Act
            var result = await _dataStore.GetAsync<TestData>(testKey);

            // Assert
            Assert.AreEqual(expected, result);
            await _mockDataManager.Received(1).ReadAsync<TestData>(testKey, _mockDataManager);
        }

        [Test]
        public async Task GetAsync_WhenKeyNotExists_ReturnsDefault()
        {
            // Arrange
            const string testKey = "nonExistentKey";
            _mockDataManager.ReadAsync<TestData>(testKey, _mockDataManager).Returns(Task.FromResult(default(TestData)));

            // Act
            var result = await _dataStore.GetAsync<TestData>(testKey);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetAsync_WhenKeyIsNull_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => _dataStore.GetAsync<TestData>(null));
        }

        #endregion

        #region SaveAsync Tests

        [Test]
        public async Task SaveAsync_WhenValidData_SavesToMemoryAndStorage()
        {
            // Arrange
            const string testKey = "testKey";
            var testData = new TestData { Id = 1, Name = "Test" };
            var serializedData = JsonConvert.SerializeObject(testData);
            _mockDataManager.Serialize(testData).Returns(serializedData);

            // Act
            await _dataStore.SaveAsync(testKey, testData);

            // Assert
            // 验证内存缓存
            var cachedValue = await _dataStore.GetAsync<TestData>(testKey);
            Assert.AreEqual(testData, cachedValue);

            // 验证存储调用
            await _mockDataManager.Received(1).WriteAsync(testKey, serializedData);
            _mockDataManager.Received(1).Serialize(testData);
        }

        [Test]
        public void SaveAsync_WhenKeyIsNull_ThrowsArgumentException()
        {
            // Arrange
            var testData = new TestData { Id = 1, Name = "Test" };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => _dataStore.SaveAsync(null, testData));
        }

        [Test]
        public void SaveAsync_WhenValueIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            const string testKey = "testKey";

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _dataStore.SaveAsync<TestData>(testKey, null));
        }

        #endregion

        #region RemoveAsync Tests

        [Test]
        public async Task RemoveAsync_WhenKeyExists_RemovesFromMemoryAndStorage()
        {
            // Arrange
            const string testKey = "testKey";
            var testData = new TestData { Id = 1, Name = "Test" };
            await _dataStore.SaveAsync(testKey, testData);
            _mockDataManager.DeleteAsync(testKey).Returns(Task.FromResult(true));

            // Act
            var result = await _dataStore.RemoveAsync(testKey);

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse((await _dataStore.ExistsAsync(testKey)));
            await _mockDataManager.Received(1).DeleteAsync(testKey);
        }

        [Test]
        public async Task RemoveAsync_WhenKeyNotExists_ReturnsFalse()
        {
            // Arrange
            const string testKey = "nonExistentKey";
            _mockDataManager.DeleteAsync(testKey).Returns(Task.FromResult(false));

            // Act
            var result = await _dataStore.RemoveAsync(testKey);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void RemoveAsync_WhenKeyIsNull_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => _dataStore.RemoveAsync(null));
        }

        #endregion

        #region ClearAsync Tests

        [Test]
        public async Task ClearAsync_ClearsMemoryAndStorage()
        {
            // Arrange
            const string testKey = "testKey";
            var testData = new TestData { Id = 1, Name = "Test" };
            await _dataStore.SaveAsync(testKey, testData);

            // Act
            await _dataStore.ClearAsync();

            // Assert
            Assert.AreEqual(0, (await _dataStore.GetAllKeysAsync()).Count());
            await _mockDataManager.Received(1).ClearAsync();
        }

        #endregion

        #region GetAllKeysAsync Tests

        [Test]
        public async Task GetAllKeysAsync_ReturnsAllMemoryKeys()
        {
            // Arrange
            const string key1 = "key1";
            const string key2 = "key2";
            await _dataStore.SaveAsync(key1, new TestData());
            await _dataStore.SaveAsync(key2, new TestData());

            // Act
            var keys = await _dataStore.GetAllKeysAsync();

            // Assert
            Assert.AreEqual(2, keys.Count());
            Assert.Contains(key1, keys.ToList());
            Assert.Contains(key2, keys.ToList());
        }

        #endregion

        #region Dispose Tests

        [Test]
        public void Dispose_ClearsMemoryCache()
        {
            // Arrange
            const string testKey = "testKey";
            _dataStore.SaveAsync(testKey, new TestData()).Wait();

            // Act
            _dataStore.Dispose();

            // Assert
            Assert.AreEqual(0, _dataStore.GetAllKeysAsync().Result.Count());
        }

        #endregion

        // 测试用数据类
        private class TestData
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override bool Equals(object obj)
            {
                return obj is TestData other &&
                       Id == other.Id &&
                       Name == other.Name;
            }

            public override int GetHashCode()
            {
                return (Id, Name).GetHashCode();
            }
        }
    }

}
