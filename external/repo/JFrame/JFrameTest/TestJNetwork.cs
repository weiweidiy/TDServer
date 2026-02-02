using JFramework.Common;
using JFramework;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Interfaces;
using System.Threading;
using System.Linq;

namespace JFrameTest
{
    /// <summary>
    /// 接口测试
    /// </summary>
    [TestFixture]
    public class TestJNetworks
    {

        private IJSocket _socketMock;
        private IJTaskCompletionSourceManager<IUnique> _taskManagerMock;
        private INetworkMessageProcessStrate _messageProcessStrateMock;
        private JNetwork _network;
        private ISocketFactory _factory;
        [SetUp]
        public void Setup()
        {
            _socketMock = Substitute.For<IJSocket>();
            _factory = Substitute.For<ISocketFactory>();
            _factory.Create().Returns(Substitute.For<IJSocket>());
            _taskManagerMock = Substitute.For<IJTaskCompletionSourceManager<IUnique>>();
            _messageProcessStrateMock = Substitute.For<INetworkMessageProcessStrate>();
            _network = new JNetwork(_factory, _taskManagerMock, _messageProcessStrateMock);
        }

        // 测试辅助类
        public class TestRequest : IJNetMessage { public string Uid { get; set; }

            public int TypeId => throw new NotImplementedException();
        }
        public class TestResponse : IJNetMessage { public string Uid { get; set; }

            public int TypeId => throw new NotImplementedException();
        }

        [Test]
        public async Task Connect_WhenSuccess_ShouldInvokeOnOpen()
        {
            // Arrange
            _socketMock.IsOpen.Returns(true);
            _factory.Create().Returns(_socketMock); //必须固定的socket
            _socketMock.When(x => x.Open()).Do(_ =>
            {
                _socketMock.onOpen += Raise.Event<Action<IJSocket>>(_socketMock);
            });

            bool opened = false;
            _network.onOpen += () => opened = true;

            // Act
            await _network.Connect("ws://test");

            // Assert
            Assert.IsTrue(opened);
        }

        [Test]
        public void SendMessage_WhenTimeout_ShouldThrowTimeoutException()
        {
            // Arrange
            _socketMock.IsOpen.Returns(true);
            _network.Socket = _socketMock;
            var request = new TestRequest { Uid = "req1"  };
            var timeout = TimeSpan.FromMilliseconds(100);

            // 模拟任务管理器行为
            var tcs = new TaskCompletionSource<IUnique>();
            _taskManagerMock.AddTask("req1").Returns(tcs);

            // 直接返回一个已经包含超时异常的任务
            _taskManagerMock.WaitingTask("req1", timeout)
                .Returns(Task.FromException<IUnique>(new TimeoutException()));

            // Act & Assert
            Assert.ThrowsAsync<TimeoutException>(async () =>
                await _network.SendMessage<TestResponse>(request, timeout));
        }

        [Test]
        public async Task SendMessage_WhenSuccess_ShouldReturnResponse()
        {
            // Arrange
            _socketMock.IsOpen.Returns(true);
            _network.Socket = _socketMock;
            var request = new TestRequest { Uid = "req1" };
            var response = new TestResponse { Uid = "resp1" };

            var byteMsg = new byte[10];
            _messageProcessStrateMock.ProcessOutMessage(request).Returns(byteMsg);

            var tcs = new TaskCompletionSource<IUnique>();
            _taskManagerMock.AddTask("req1").Returns(tcs);
            _taskManagerMock.WaitingTask("req1", null).Returns(tcs.Task);

            // 需要在一个单独的线程中设置结果，模拟异步响应
            Task.Run(() =>
            {
                Task.Delay(50).Wait();
                tcs.SetResult(response);
            });

            // Act
            var result = await _network.SendMessage<TestResponse>(request);

            // Assert
            Assert.AreEqual("resp1", result.Uid);
        }

        [Test]
        public void Socket_OnBinary_ShouldCompleteMatchingTask()
        {
            // Arrange
            var response = new TestResponse { Uid = "resp1" };
            var byteData = new byte[10];
            _messageProcessStrateMock.ProcessComingMessage(byteData).Returns(response);

            var tcs = new TaskCompletionSource<IUnique>();
            _taskManagerMock.GetTask("resp1").Returns(tcs);

            // Act
            _network.Socket_OnBinary(_socketMock, byteData);

            // Assert
            Assert.AreEqual(response, tcs.Task.Result);
        }
    }

    /// <summary>
    /// 集成测试
    /// </summary>

    [TestFixture]
    public class TestJNetworks2
    {
        private IJSocket _socketMock;
        private IJTaskCompletionSourceManager<IUnique> _taskManager;
        private INetworkMessageProcessStrate _messageProcessor;
        private JNetwork _network;
        private ISocketFactory _factory;

        // 测试辅助类
        public class TestRequest : IJNetMessage
        {
            public string Uid { get; set; }

            public int TypeId => throw new NotImplementedException();
        }
        public class TestResponse : IJNetMessage
        {
            public string Uid { get; set; }

            public int TypeId => throw new NotImplementedException();
        }

        [SetUp]
        public void Setup()
        {
            _socketMock = Substitute.For<IJSocket>();
            _factory = Substitute.For<ISocketFactory>();
            _factory.Create().Returns(Substitute.For<IJSocket>());
            _taskManager = new JTaskCompletionSourceManager<IUnique>(); // 使用真实实现
            _messageProcessor = Substitute.For<INetworkMessageProcessStrate>();
            _network = new JNetwork(_factory, _taskManager, _messageProcessor);
        }

        [Test]
        public async Task Connect_ShouldHandleFullLifecycle()
        {
            // Arrange
            var url = "ws://testserver";
            bool opened = false, closed = false;
            _network.onOpen += () => opened = true;
            _network.onClose += (s, c) => closed = true;

            _factory.Create().Returns(_socketMock); //必须固定的socket
            _socketMock.When(x => x.Open()).Do(_ =>
            {
                _socketMock.onOpen += Raise.Event<Action<IJSocket>>(_socketMock);
            });

            // Act
            await _network.Connect(url);

            // Mock连接成功
   

            // Mock连接关闭
            _network.Socket.onClosed += Raise.Event<Action<IJSocket, SocketStatusCodes, string>>(
                _network.Socket, SocketStatusCodes.NormalClosure, "Closed");

            // Assert
            Assert.IsTrue(opened);
            Assert.IsTrue(closed);
            Assert.IsFalse(_network.IsConnecting());
        }

        [Test]
        public async Task SendMessage_ShouldCompleteFullRequestResponseFlow()
        {
            // Arrange
            var request = new TestRequest { Uid = "req_123" };
            var response = new TestResponse { Uid = "resp_123" };
            var responseBytes = Encoding.UTF8.GetBytes("response_data");

            // Mock连接状态
            _socketMock.IsOpen.Returns(true);
            _network.Socket = _socketMock;

            // Mock消息处理
            _messageProcessor.ProcessOutMessage(request).Returns(new byte[10]);
            _messageProcessor.ProcessComingMessage(responseBytes).Returns(response);

            // 模拟任务管理器行为（关键修复点）

            // Act
            var sendTask = _network.SendMessage<TestResponse>(request);

            // 模拟服务器响应（两种方式任选其一）

            // 方式1：直接设置任务结果（推荐）
            _taskManager.SetResult(request.Uid, response);

            // 方式2：或者触发socket事件（需要确保事件处理器已正确设置）
            // _socketMock.onBinary += Raise.Event<Action<IJSocket, byte[]>>(_socketMock, responseBytes);

            // Assert
            var result = await sendTask;
            Assert.AreEqual(response.Uid, result.Uid);
        }

        //[Test]
        //public void Network_ShouldHandleErrorScenarios()
        //{
        //    // Arrange
        //    var errorMsg = "Connection error";
        //    string receivedError = null;
        //    _network.onError += msg => receivedError = msg;

        //    // Act - 模拟连接错误
        //    _socketMock.onError += Raise.Event<Action<IJSocket, string>>(_socketMock, errorMsg);

        //    // Assert
        //    Assert.AreEqual(errorMsg, receivedError);
        //}
        [Test]
        public void SendMessage_ShouldTimeoutWhenNoResponse()
        {
            // Arrange
            var request = new TestRequest { Uid = "timeout_test" };


            _factory.Create().Returns(_socketMock);
            _socketMock.IsOpen.Returns(true);
            _network.Socket = _socketMock;

            _messageProcessor.ProcessOutMessage(request).Returns(new byte[10]);

            
            // Act & Assert
            Assert.ThrowsAsync<TimeoutException>(() =>_network.SendMessage<TestResponse>(request, TimeSpan.FromMilliseconds(100)));
        }

    }
}
