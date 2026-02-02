using System;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace JFramework.Tests
{
    [TestFixture]
    public class BaseRunableTests
    {
        // 测试用的具体实现类
        private class TestRunable : BaseRunable
        {
            public int OnStartCallCount { get; private set; }
            public int OnStopCallCount { get; private set; }
            public int OnUpdateCallCount { get; private set; }
            public RunableExtraData StartData { get; private set; }
            public RunableExtraData UpdateData { get; private set; }

            protected override void OnStart(RunableExtraData extraData)
            {
                OnStartCallCount++;
                StartData = extraData;
            }

            protected override void OnStop()
            {
                OnStopCallCount++;
            }

            protected override void OnUpdate(RunableExtraData extraData)
            {
                OnUpdateCallCount++;
                UpdateData = extraData;
            }
        }

        private TestRunable _runable;
        private RunableExtraData _testData;

        [SetUp]
        public void Setup()
        {
            _runable = new TestRunable();
            _testData = new RunableExtraData();
        }

        [Test]
        public async Task Start_WhenNotRunning_SetsPropertiesAndCallsOnStart()
        {
            // Act
            var startTask = _runable.Start(_testData);

            // Assert
            Assert.IsTrue(_runable.IsRunning);
            Assert.AreEqual(1, _runable.OnStartCallCount);
            Assert.AreSame(_testData, _runable.ExtraData);
            Assert.AreSame(_testData, _runable.StartData);

            // Cleanup
            _runable.Stop();
            await startTask;
        }

        [Test]
        public void Start_WhenAlreadyRunning_ThrowsException()
        {
            // Arrange
            _runable.Start(_testData); // 不等待完成

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _runable.Start(new RunableExtraData()));

            StringAssert.Contains("is running", ex.Message);
            Assert.IsTrue(_runable.IsRunning);

            // Cleanup
            _runable.Stop();
        }

        [Test]
        public async Task Stop_WhenRunning_SetsPropertiesAndCallsOnStop()
        {
            // Arrange
            var startTask = _runable.Start(_testData);

            // Act
            _runable.Stop();
            await startTask; // 等待异步完成

            // Assert
            Assert.IsFalse(_runable.IsRunning);
            Assert.AreEqual(1, _runable.OnStopCallCount);
        }

        [Test]
        public void Stop_WhenNotRunning_DoesNothing()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => _runable.Stop());
            Assert.AreEqual(0, _runable.OnStopCallCount);
        }

        [Test]
        public async Task Stop_CompletesStartTask()
        {
            // Arrange
            var startTask = _runable.Start(_testData);

            // Pre-assert
            Assert.IsFalse(startTask.IsCompleted);

            // Act
            _runable.Stop();

            // Assert
            await startTask;
            Assert.IsTrue(startTask.IsCompleted);
        }

        [Test]
        public async Task Stop_TriggersOnCompleteEvent()
        {
            // Arrange
            var eventHandler = Substitute.For<Action<IRunable>>();
            _runable.onComplete += eventHandler;

            var startTask = _runable.Start(_testData);

            // Act
            _runable.Stop();
            await startTask;

            // Assert
            eventHandler.Received(1).Invoke(_runable);
        }

        [Test]
        public void Update_CallsOnUpdateWithCorrectData()
        {
            // Arrange
            var updateData = new RunableExtraData();

            // Act
            _runable.Update(updateData);

            // Assert
            Assert.AreEqual(1, _runable.OnUpdateCallCount);
            Assert.AreSame(updateData, _runable.UpdateData);
        }

        [Test]
        public async Task Start_WithNullExtraData_HandlesCorrectly()
        {
            // Act
            var startTask = _runable.Start(null);

            // Assert
            Assert.IsNull(_runable.ExtraData);
            Assert.IsNull(_runable.StartData);

            // Cleanup
            _runable.Stop();
            await startTask;
        }

        [Test]
        public async Task Stop_WhenCalledMultipleTimes_OnlyStopsOnce()
        {
            // Arrange
            var startTask = _runable.Start(_testData);

            // Act
            _runable.Stop();
            _runable.Stop(); // 第二次调用

            // Assert
            await startTask;
            Assert.AreEqual(1, _runable.OnStopCallCount);
        }

        [Test]
        public async Task OnComplete_WithMultipleSubscribers_AllTriggered()
        {
            // Arrange
            var handler1 = Substitute.For<Action<IRunable>>();
            var handler2 = Substitute.For<Action<IRunable>>();
            _runable.onComplete += handler1;
            _runable.onComplete += handler2;

            var startTask = _runable.Start(_testData);

            // Act
            _runable.Stop();
            await startTask;

            // Assert
            handler1.Received(1).Invoke(_runable);
            handler2.Received(1).Invoke(_runable);
        }

        [Test]
        public async Task Stop_AfterCompletion_DoesNotInvokeOnStopAgain()
        {
            // Arrange
            var startTask = _runable.Start(_testData);
            _runable.Stop();
            await startTask;

            // Act
            _runable.Stop();

            // Assert
            Assert.AreEqual(1, _runable.OnStopCallCount);
        }

        [Test]
        public void Update_WhenNotRunning_StillProcessesUpdate()
        {
            // Arrange
            var updateData = new RunableExtraData();

            // Act
            _runable.Update(updateData);

            // Assert
            Assert.AreEqual(1, _runable.OnUpdateCallCount);
            Assert.AreSame(updateData, _runable.UpdateData);
        }
    }



}