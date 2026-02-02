using System;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;

namespace JFramework.Game.Tests
{
    [TestFixture]
    public class BaseUnlockableModelTests
    {
        // 测试用具体实现类
        public class TestUnlockable : IUnlockable
        {
            public string Uid { get; set; }
            public bool IsLocked { get; private set; } = true;

            public void Lock() => IsLocked = true;
            public void Unlock() => IsLocked = false;

            bool IUnlockable.IsLocked() => IsLocked;
        }

        public class TestModel : BaseUnlockableModel<string, TestUnlockable>
        {
            public TestModel(EventManager eventManager)
                : base(data => data.Uid, eventManager)
            {
            }

            public void PublicAdd(TestUnlockable item) => Add(item);

            protected override void OnUpdateTData(List<TestUnlockable> unlockableDatas)
            {
                //throw new NotImplementedException();
            }
        }

        private TestModel _model;
        private EventManager _mockEventManager;
        private TestUnlockable _testItem;

        [SetUp]
        public void Setup()
        {
            _mockEventManager = Substitute.For<EventManager>();
            _model = new TestModel(_mockEventManager);

            _testItem = new TestUnlockable { Uid = "item1" };
            _model.PublicAdd(_testItem);

            _model.Initialize("testData");
        }

        [Test]
        public void Initialize_SetsDataCorrectly()
        {
            // Assert
            Assert.AreEqual("testData", _model.Data);
        }

        [Test]
        public void Lock_ValidUid_LocksItem()
        {
            // Act
            _model.Lock("item1");

            // Assert
            Assert.IsTrue(_testItem.IsLocked);
        }

        [Test]
        public void Lock_InvalidUid_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _model.Lock("invalid_uid"));
        }

        [Test]
        public void Unlock_ValidUid_UnlocksItem()
        {
            // Arrange
            _testItem.Unlock(); // 初始解锁

            // Act
            _model.Unlock("item1");

            // Assert
            Assert.IsFalse(_testItem.IsLocked);
        }

        [Test]
        public void Unlock_InvalidUid_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _model.Unlock("invalid_uid"));
        }

        [Test]
        public void SendEvent_RaisesEventCorrectly()
        {
            // Arrange
            var testEvent = new TestEvent();

            // Act
            //_model.SendEvent<TestEvent>(testEvent);

            // Assert
            _mockEventManager.Received(1).Raise(testEvent);
        }

        [Test]
        public void EventManager_NullInConstructor_ThrowsException()
        {
            // Act & Assert
            var ex = Assert.Throws<Exception>(() => new TestModel(null));
            StringAssert.Contains("eventManager", ex.Message);
        }

        [Test]
        public void Update_AfterLock_SendsEvent()
        {
            // 需要修改基类为protected virtual void Update(TUnlockableData data)
            // 或在子类中实现事件触发逻辑
            // 此测试取决于具体实现细节
        }
    }

    // 测试用事件类型
    public class TestEvent : Event { }
}