using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using JFramework;

namespace JFrameTest
{
    [TestFixture]
    internal class TestJTaskCompletionSourceManager
    {
        private JTaskCompletionSourceManager<string> _taskManager;

        [SetUp]
        public void Setup()
        {
            _taskManager = new JTaskCompletionSourceManager<string>();
        }

        [TearDown]
        public void TearDown()
        {
            // 清理所有任务
            _taskManager = null;
        }

        [Test]
        public void AddTask_ShouldAddNewTask_WhenUidIsUnique()
        {
            // Arrange
            var uid = "test-uid";

            // Act
            var result = _taskManager.AddTask(uid);

            // Assert
            Assert.IsNotNull(result); // 返回 null 表示添加成功
            var task = _taskManager.GetTask(uid);
            Assert.IsNotNull(task);
        }

        [Test]
        public void AddTask_ShouldReturnExistingTcs_WhenUidExists()
        {
            // Arrange
            var uid = "test-uid";
            _taskManager.AddTask(uid);

            // Act
            var result = _taskManager.AddTask(uid);

            // Assert
            Assert.IsNull(result); // 返回非 null 表示已存在
        }

        [Test]
        public void RemoveTask_ShouldReturnTrue_WhenTaskExists()
        {
            // Arrange
            var uid = "test-uid";
            _taskManager.AddTask(uid);

            // Act
            var result = _taskManager.RemoveTask(uid);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(_taskManager.GetTask(uid));
        }

        [Test]
        public void RemoveTask_ShouldReturnFalse_WhenTaskNotExists()
        {
            // Arrange
            var uid = "non-existent-uid";

            // Act
            var result = _taskManager.RemoveTask(uid);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetTask_ShouldReturnNull_WhenTaskNotExists()
        {
            // Arrange
            var uid = "non-existent-uid";

            // Act
            var result = _taskManager.GetTask(uid);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task SetResult_ShouldCompleteTask_WhenTaskExists()
        {
            // Arrange
            var uid = "test-uid";
            var expectedResult = "test-result";
            _taskManager.AddTask(uid);

            // Act
            _taskManager.SetResult(uid, expectedResult);
            var task = _taskManager.GetTask(uid);
            var result = await task.Task;

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void SetResult_ShouldThrow_WhenTaskNotExists()
        {
            // Arrange
            var uid = "non-existent-uid";

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _taskManager.SetResult(uid, "any"));
        }

        [Test]
        public void SetException_ShouldSetException_WhenTaskExists()
        {
            // Arrange
            var uid = "test-uid";
            var expectedException = new Exception("test exception");
            _taskManager.AddTask(uid);

            // Act
            _taskManager.SetException(uid, expectedException);
            var task = _taskManager.GetTask(uid);

            // Assert
            Assert.IsTrue(task.Task.IsFaulted);
            Assert.AreEqual(expectedException.Message, task.Task.Exception.InnerException.Message);
        }

        [Test]
        public void SetException_ShouldThrow_WhenTaskNotExists()
        {
            // Arrange
            var uid = "non-existent-uid";

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _taskManager.SetException(uid, new Exception()));
        }

        [Test]
        public async Task WaitingTask_ShouldReturnResult_WhenTaskCompletes()
        {
            // Arrange
            var uid = "test-uid";
            var expectedResult = "test-result";
            _taskManager.AddTask(uid);

            // Act
            _taskManager.SetResult(uid, expectedResult);
            var result = await _taskManager.WaitingTask(uid);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void WaitingTask_ShouldThrowTimeoutException_WhenTaskTimesOut()
        {
            // Arrange
            var uid = "test-uid";
            _taskManager.AddTask(uid);
            var timeout = TimeSpan.FromMilliseconds(100); // 很短的超时时间

            // Act & Assert
            Assert.ThrowsAsync<TimeoutException>(async () => await _taskManager.WaitingTask(uid, timeout));
        }

        [Test]
        public void WaitingTask_ShouldThrow_WhenTaskHasException()
        {
            // Arrange
            var uid = "test-uid";
            var expectedException = new Exception("test exception");
            _taskManager.AddTask(uid);

            // Act
            _taskManager.SetException(uid, expectedException);

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await _taskManager.WaitingTask(uid));
        }

        [Test]
        public async Task WaitingTask_ShouldComplete_WhenResultIsSetBeforeWaiting()
        {
            // Arrange
            var uid = "test-uid";
            var expectedResult = "test-result";
            _taskManager.AddTask(uid);
            _taskManager.SetResult(uid, expectedResult);

            // Act
            var result = await _taskManager.WaitingTask(uid);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void MultipleWaitingTasks_ShouldAllComplete_WhenResultIsSet()
        {
            // Arrange
            var uid = "test-uid";
            var expectedResult = "test-result";
            _taskManager.AddTask(uid);

            // Act
            var task1 = _taskManager.WaitingTask(uid);
            var task2 = _taskManager.WaitingTask(uid);
            _taskManager.SetResult(uid, expectedResult);

            // Assert
            Assert.DoesNotThrowAsync(async () =>
            {
                var result1 = await task1;
                var result2 = await task2;
                Assert.AreEqual(expectedResult, result1);
                Assert.AreEqual(expectedResult, result2);
            });
        }
    }
}
