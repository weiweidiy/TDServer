using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using JFramework.Game;
using JFramework;

namespace JFramework.Tests
{
    [TestFixture]
    public class JCombatPlayerTests
    {
        private TestCombatPlayer _player;
        private IObjectPool _mockPool;
        private JCombatTurnBasedEventRunner _mockRunner;
        private RunableExtraData _mockExtraData;
        private JCombatTurnBasedReportData _testReport;
        private TaskCompletionSource<bool> _testTcs;

        [SetUp]
        public void Setup()
        {
            // 创建模拟对象
            _mockPool = Substitute.For<IObjectPool>();
            _mockRunner = Substitute.For<JCombatTurnBasedEventRunner>();
            _mockExtraData = Substitute.For<RunableExtraData>();
            _testTcs = new TaskCompletionSource<bool>();

            // 配置对象池行为
            _mockPool.Rent<JCombatTurnBasedEventRunner>().Returns(_mockRunner);
            _mockPool.Rent<RunableExtraData>().Returns(_mockExtraData);

            // 创建测试报告数据
            _testReport = new JCombatTurnBasedReportData
            {
                winnerTeamUid = "team1",
                events = new List<JCombatTurnBasedEvent>
                {
                    new JCombatTurnBasedEvent { Uid = "Attack" },
                    new JCombatTurnBasedEvent { Uid = "Defend" },
                    new JCombatTurnBasedEvent { Uid = "Heal" }
                }
            };

            // 创建测试玩家实例
            _player = new TestCombatPlayer(_mockPool);
        }

        [Test]
        public void Constructor_WithPool_SetsPoolProperty()
        {
            // Act
            var player = new TestCombatPlayer(_mockPool);

            // Assert
            Assert.AreSame(_mockPool, player.Pool);
        }

        [Test]
        public void Constructor_WithoutPool_SetsPoolToNull()
        {
            // Act
            var player = new TestCombatPlayer();

            // Assert
            Assert.IsNull(player.Pool);
        }

        [Test]
        public void Play_SetsReportDataProperty()
        {
            // Act
            _player.Play(_testReport);

            // Assert
            Assert.AreSame(_testReport, _player.ReportData);
        }

        [Test]
        public void Play_CallsOnStartPlayWithEvents()
        {
            // Act
            _player.Play(_testReport);

            // Assert
            Assert.IsTrue(_player.OnStartPlayCalled);
            CollectionAssert.AreEqual(_testReport.events, _player.ReceivedEvents);
        }

        [Test]
        public async Task Play_WithPool_ProcessesAllEvents()
        {
            // 设置模拟运行器完成任务
            _mockRunner.Start(Arg.Any<RunableExtraData>()).Returns(Task.CompletedTask);

            // Act
            _player.Play(_testReport);

            // 等待事件处理完成
            await Task.Delay(100);

            // Assert
            _mockPool.Received(3).Rent<JCombatTurnBasedEventRunner>();
            _mockPool.Received(3).Rent<RunableExtraData>();
            await _mockRunner.Received(3).Start(Arg.Any<RunableExtraData>());
        }

        //[Test]
        //public async Task Play_WithPool_SetsEventDataCorrectly()
        //{
        //    // 设置模拟运行器完成任务
        //    _mockRunner.Start(Arg.Any<RunableExtraData>()).Returns(Task.CompletedTask);

        //    // Act
        //    _player.Play(_testReport);

        //    // 等待事件处理完成
        //    await Task.Delay(100);

        //    // 验证每个事件的设置
        //    Received.InOrder(() => {
        //        _mockExtraData.Data = _testReport.events[0];
        //        _mockRunner.Start(Arg.Is<RunableExtraData>(d => ((JCombatTurnBasedEvent)d.Data).Uid == _testReport.events[0].Uid));

        //        _mockExtraData.Data = _testReport.events[1];
        //        _mockRunner.Start(Arg.Is<RunableExtraData>(d => ((JCombatTurnBasedEvent)d.Data).Uid == _testReport.events[1].Uid));

        //        _mockExtraData.Data = _testReport.events[2];
        //        _mockRunner.Start(Arg.Is<RunableExtraData>(d => ((JCombatTurnBasedEvent)d.Data).Uid == _testReport.events[2].Uid));
        //    });
        //}

        //[Test]
        //public async Task Play_WithoutPool_ProcessesAllEvents()
        //{
        //    // Arrange
        //    var player = new TestCombatPlayer(); // 无对象池

        //    // Act
        //    player.Play(_testReport);

        //    // 等待事件处理完成
        //    await Task.Delay(100);

        //    // Assert
        //    Assert.AreEqual(3, player.CreatedRunners.Count);
        //    Assert.AreEqual(3, player.CreatedExtraData.Count);
        //    Assert.AreEqual(3, player.StartedRunners.Count);
        //}

        //[Test]
        //public async Task Play_WithPool_ReleasesResources()
        //{
        //    // 设置模拟运行器完成任务
        //    _mockRunner.Start(Arg.Any<RunableExtraData>()).Returns(Task.CompletedTask);

        //    // Act
        //    _player.Play(_testReport);

        //    // 等待事件处理完成
        //    await Task.Delay(100);

        //    // Assert
        //    _mockPool.Received(3).Return(_mockRunner);
        //    _mockPool.Received(3).Return(_mockExtraData);
        //    _mockRunner.Received(3).Dispose();
        //}

        //[Test]
        //public async Task Play_WithoutPool_DisposesResources()
        //{
        //    // Arrange
        //    var player = new TestCombatPlayer(); // 无对象池

        //    // Act
        //    player.Play(_testReport);

        //    // 等待事件处理完成
        //    await Task.Delay(100);

        //    // Assert
        //    foreach (var runner in player.CreatedRunners)
        //    {
        //        runner.Received().Dispose();
        //    }
        //}

        [Test]
        public void RePlay_UsesSameReportData()
        {
            // Arrange
            _player.Play(_testReport);

            // Act
            _player.RePlay();

            // Assert
            Assert.AreSame(_testReport, _player.ReportData);
            Assert.AreEqual(2, _player.PlayCallCount);
        }

        [Test]
        public void SetScale_ChangesScaleValue()
        {
            // Act
            _player.SetScale(2.5f);

            // Assert
            Assert.AreEqual(2.5f, _player.GetScale());
        }

        [Test]
        public void GetScale_ReturnsCurrentScale()
        {
            // Arrange
            _player.SetScale(0.75f);

            // Act & Assert
            Assert.AreEqual(0.75f, _player.GetScale());
        }

        //[Test]
        //public async Task OnStart_TriggersPlayWithReportData()
        //{
        //    // Arrange
        //    var extraData = new RunableExtraData { Data = _testReport };

        //    // Act
        //    await _player.Start(extraData);

        //    // Assert
        //    Assert.AreSame(_testReport, _player.ReportData);
        //    Assert.IsTrue(_player.PlayCalled);
        //}

        [Test]
        public void OnStart_WithInvalidData_DoesNotPlay()
        {
            // Arrange
            var extraData = new RunableExtraData { Data = "invalid data" };

            // Act
           ;

            // Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _player.Start(extraData));
        }

        [Test]
        public async Task OnStart_WithCustomTcs_UsesProvidedTcs()
        {
            // Arrange
            var extraData = new RunableExtraData { Data = _testReport };
            var customTcs = new TaskCompletionSource<bool>();

            // Act
            var startTask = _player.Start(extraData, customTcs);

            // Assert - 验证使用了自定义Tcs
            Assert.AreSame(customTcs, _player.CurrentTcs);

            // 清理
            _player.Stop();
            await startTask;
        }

        [Test]
        public void OnStop_CallsBaseImplementation()
        {
            // Arrange
            _player.Start(new RunableExtraData { Data = _testReport });

            // Act
            _player.Stop();

            // Assert
            Assert.IsFalse(_player.IsRunning);
        }

        [Test]
        public async Task Play_WithZeroEvents_DoesNotProcessAnyEvents()
        {
            // Arrange
            var emptyReport = new JCombatTurnBasedReportData
            {
                winnerTeamUid = "team1",
                events = new List<JCombatTurnBasedEvent>()
            };

            // Act
            _player.Play(emptyReport);
            await Task.Delay(50); // 确保处理完成

            // Assert
            _mockPool.DidNotReceive().Rent<JCombatTurnBasedEventRunner>();
            _mockPool.DidNotReceive().Rent<RunableExtraData>();
            await _mockRunner.DidNotReceive().Start(Arg.Any<RunableExtraData>());
        }

        [Test]
        public async Task Play_WithSingleEvent_ProcessesCorrectly()
        {
            // Arrange
            var singleEventReport = new JCombatTurnBasedReportData
            {
                winnerTeamUid = "team1",
                events = new List<JCombatTurnBasedEvent> { new JCombatTurnBasedEvent { Uid = "Special" } }
            };

            // 设置模拟运行器完成任务
            _mockRunner.Start(Arg.Any<RunableExtraData>()).Returns(Task.CompletedTask);

            // Act
            _player.Play(singleEventReport);
            await Task.Delay(50); // 确保处理完成

            // Assert
            _mockPool.Received(1).Rent<JCombatTurnBasedEventRunner>();
            _mockPool.Received(1).Rent<RunableExtraData>();
            await _mockRunner.Received(1).Start(Arg.Any<RunableExtraData>());
        }

        //[Test]
        //public async Task Play_WhenEventRunnerThrowsException_ContinuesProcessing()
        //{
        //    // Arrange
        //    var exception = new Exception("Test exception");
        //    _mockRunner.Start(Arg.Any<RunableExtraData>())
        //        .Returns(Task.CompletedTask, Task.FromException(exception), Task.CompletedTask);

        //    // Act
        //    _player.Play(_testReport);

        //    // 等待事件处理完成
        //    await Task.Delay(100);

        //    // Assert - 确保所有事件都被处理
        //    await _mockRunner.Received(3).Start(Arg.Any<RunableExtraData>());
        //}

        [Test]
        public async Task GetEventRunner_CanBeOverridden()
        {
            // Arrange
            var customRunner = Substitute.For<JCombatTurnBasedEventRunner>();
            var player = new TestCombatPlayer(_mockPool)
            {
                OverrideGetEventRunner = true,
                CustomRunner = customRunner
            };

            // Act
            player.Play(_testReport);
            await Task.Delay(100);

            // Assert
            Assert.AreSame(customRunner, player.GetEventRunnerResult);
        }

        [Test]
        public async Task GetRunableData_CanBeOverridden()
        {
            // Arrange
            var customData = new RunableExtraData();
            var player = new TestCombatPlayer(_mockPool)
            {
                OverrideGetRunableData = true,
                CustomData = customData
            };

            // Act
            player.Play(_testReport);
            await Task.Delay(100);

            // Assert
            Assert.AreSame(customData, player.GetRunableDataResult);
        }

        [Test]
        public async Task ReleaseRunner_CanBeOverridden()
        {
            // Arrange
            var player = new TestCombatPlayer(_mockPool)
            {
                OverrideReleaseRunner = true
            };

            // Act
            player.Play(_testReport);
            await Task.Delay(100);

            // Assert
            Assert.IsTrue(player.ReleaseRunnerCalled);
        }

        [Test]
        public async Task OnStartPlay_CanBeOverridden()
        {
            // Arrange
            var player = new TestCombatPlayer(_mockPool)
            {
                OverrideOnStartPlay = true
            };

            // Act
            player.Play(_testReport);
            await Task.Delay(100);

            // Assert
            Assert.IsTrue(player.CustomOnStartPlayCalled);
            Assert.IsFalse(player.OnStartPlayCalled);
        }

        // 测试用的具体实现类
        private class TestCombatPlayer : JCombatTurnBasedPlayer
        {
            public int PlayCallCount { get; private set; }
            public bool PlayCalled => PlayCallCount > 0;
            public List<JCombatTurnBasedEventRunner> CreatedRunners { get; } = new List<JCombatTurnBasedEventRunner>();
            public List<RunableExtraData> CreatedExtraData { get; } = new List<RunableExtraData>();
            public List<RunableExtraData> StartedRunners { get; } = new List<RunableExtraData>();
            public IObjectPool Pool => pool;
            public JCombatTurnBasedReportData ReportData => reportData;
            public bool OnStartPlayCalled { get; private set; }
            public List<JCombatTurnBasedEvent> ReceivedEvents { get; } = new List<JCombatTurnBasedEvent>();
            public TaskCompletionSource<bool> CurrentTcs { get; private set; }

            // 用于测试可重写方法
            public bool OverrideGetEventRunner { get; set; }
            public JCombatTurnBasedEventRunner CustomRunner { get; set; } = Substitute.For<JCombatTurnBasedEventRunner>();
            public JCombatTurnBasedEventRunner GetEventRunnerResult { get; private set; }

            public bool OverrideGetRunableData { get; set; }
            public RunableExtraData CustomData { get; set; }
            public RunableExtraData GetRunableDataResult { get; private set; }

            public bool OverrideReleaseRunner { get; set; }
            public bool ReleaseRunnerCalled { get; private set; }

            public bool OverrideOnStartPlay { get; set; }
            public bool CustomOnStartPlayCalled { get; private set; }

            public TestCombatPlayer() { }
            public TestCombatPlayer(IObjectPool objPool) : base(objPool) { }

            public override void Play(JCombatTurnBasedReportData report)
            {
                PlayCallCount++;
                base.Play(report);
            }

            protected override void OnStartPlay(List<JCombatTurnBasedEvent> events)
            {
                if (OverrideOnStartPlay)
                {
                    CustomOnStartPlayCalled = true;
                    // 自定义实现
                    base.OnStartPlay(events);
                }
                else
                {
                    OnStartPlayCalled = true;
                    ReceivedEvents.AddRange(events);
                    base.OnStartPlay(events);
                }
            }

            protected override JCombatTurnBasedEventRunner GetEventRunner()
            {
                if (OverrideGetEventRunner)
                {
                    GetEventRunnerResult = CustomRunner;
                    return CustomRunner;
                }

                var runner = base.GetEventRunner();
                if (runner != null && pool == null) // 如果没有使用对象池
                {
                    CreatedRunners.Add(runner);
                }
                return runner;
            }

            protected override RunableExtraData GetRunableData()
            {
                if (OverrideGetRunableData)
                {
                    GetRunableDataResult = CustomData;
                    return CustomData;
                }

                var data = base.GetRunableData();
                if (data != null && pool == null) // 如果没有使用对象池
                {
                    CreatedExtraData.Add(data);
                }
                return data;
            }

            protected override void ReleaseRunner(JCombatTurnBasedEventRunner runner, RunableExtraData extraData)
            {
                if (OverrideReleaseRunner)
                {
                    ReleaseRunnerCalled = true;
                    // 自定义实现
                    base.ReleaseRunner(runner, extraData);
                }
                else
                {
                    if (pool == null)
                    {
                        StartedRunners.Add(extraData);
                    }
                    base.ReleaseRunner(runner, extraData);
                }
            }

            public override async Task Start(RunableExtraData extraData, TaskCompletionSource<bool> tcs = null)
            {
                CurrentTcs = tcs;
                await base.Start(extraData, tcs);
            }
        }
    }

}