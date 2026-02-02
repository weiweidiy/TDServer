using JFramework;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JFrameTest
{
    internal class TestMessageJsonTypeResolver
    {
        [TestFixture]
        public class MessageJsonTypeResolverTests
        {
            private ISerializer _mockSerializer;
            private IDeserializer _mockDeserializer;
            private JNetMessageJsonTypeResolver _resolver;

            [SetUp]
            public void Setup()
            {
                _mockSerializer = Substitute.For<ISerializer>();
                _mockDeserializer = Substitute.For<IDeserializer>();
                _resolver = new JNetMessageJsonTypeResolver(_mockDeserializer);
            }

            [Test]
            public void RegisterMessageType_ShouldRegisterTypeCorrectly()
            {
                // Arrange
                var messageId = 123;
                var messageType = typeof(TestMessage);

                // Act
                var result = _resolver.RegisterMessageType(messageId, messageType);

                // Assert
                Assert.AreSame(_resolver, result); // 测试是否返回自身以实现链式调用
                                                   // 可以通过反射检查内部字典是否包含注册的类型
            }

            //[Test]
            //public void ResolveMessageType_ShouldReturnCorrectType_WhenMessageIdIsRegistered()
            //{
            //    // Arrange
            //    var messageId = 123;
            //    var expectedType = typeof(TestMessage);
            //    _resolver.RegisterMessageType(messageId, expectedType);

            //    var json = "{\"TypeId\":123}";
            //    var data = Encoding.UTF8.GetBytes(json);

            //    var mockTypeId = Substitute.For<ITypeId>();
            //    mockTypeId.TypeId.Returns(messageId);
            //    _mockSerializer.ToObject<ITypeId>(json).Returns(mockTypeId);

            //    // Act
            //    var result = _resolver.ResolveMessageType(data);

            //    // Assert
            //    Assert.AreEqual(expectedType, result);
            //}

            //[Test]
            //public void ResolveMessageType_ShouldThrowException_WhenMessageIdIsNotRegistered()
            //{
            //    // Arrange
            //    var unregisteredMessageId = 999;
            //    var json = "{\"TypeId\":999}";
            //    var data = Encoding.UTF8.GetBytes(json);

            //    var mockTypeId = Substitute.For<ITypeId>();
            //    mockTypeId.TypeId.Returns(unregisteredMessageId);
            //    _mockSerializer.ToObject<ITypeId>(json).Returns(mockTypeId);

            //    // Act & Assert
            //    var ex = Assert.Throws<InvalidOperationException>(() => _resolver.ResolveMessageType(data));
            //    Assert.AreEqual($"Unknown message ID: {unregisteredMessageId}", ex.Message);
            //}

            [Test]
            public void ResolveMessageType_ShouldThrowException_WhenInvalidUtf8DataReceived()
            {
                // Arrange
                // 创建一个肯定会失败的UTF-8序列
                var invalidData = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };

                // Act & Assert
                var ex = Assert.Throws<InvalidOperationException>(() => _resolver.ResolveMessageType(invalidData));
                Assert.That(ex.Message, Does.Contain("Unknown message ID"));
            }

            [Test]
            public void ResolveMessageType_ShouldReturn_WhenUtf8DataReceived()
            {
                var data = new TestMessage() { TypeId = 1 };
                var json = JsonConvert.SerializeObject(data);
                var byteData = Encoding.UTF8.GetBytes((string)json);

                var strData = Encoding.UTF8.GetString(byteData);
                //var obj = JsonConvert.DeserializeObject<TestMessage>(strData);

                var serializer = new JsonNetSerializer();
                var messageResolve = new JNetMessageJsonTypeResolver(serializer);
                messageResolve.RegisterMessageType(1, data.GetType());

                var type = messageResolve.ResolveMessageType(byteData);
                var obj = serializer.ToObject(strData, type) as TestMessage;


                Assert.AreEqual(obj.value, 10);
            }

            // 测试辅助类

            //public class TestJsonSerializer : IJsonSerializer
            //{
            //    public string ToJson(object obj)
            //    {
            //        return JsonConvert.SerializeObject(obj);
            //    }

            //    public T ToObject<T>(string str)
            //    {
            //        return JsonConvert.DeserializeObject<T>(str);
            //    }

            //    public object ToObject(string json, Type type)
            //    {
            //        return JsonConvert.DeserializeObject(json, type);
            //    }

            //    public object ToObject(byte[] bytes, Type type)
            //    {
            //        throw new NotImplementedException();
            //    }
            //}

            private class TestMessage : ITypeId
            {
                public int TypeId { get; set; }
                public int value = 10;
            }
        }
    }
}
