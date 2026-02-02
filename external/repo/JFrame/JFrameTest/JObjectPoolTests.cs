using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace JFramework.Tests
{
    [TestFixture]
    public class JObjectPoolTests
    {
        // 测试用的抽象类实现
        public class TestObjectPool : JObjectPool
        {
            public Dictionary<Type, bool> RegisteredTypes { get; } = new Dictionary<Type, bool>();

            public TestObjectPool(ITypeRegister typeRegister,
                Func<Type, Action<object>> rentDelegateFactory = null,
                Func<Type, Action<object>> returnDelegateFactory = null,
                Func<Type, Action<object>> releaseDelegateFactory = null)
                : base(typeRegister, rentDelegateFactory, returnDelegateFactory, releaseDelegateFactory)
            {
            }

            protected override void Regist<T>(Action<T> onRent, Action<T> onReturn, Action<T> onRelease)
            {
                RegisteredTypes[typeof(T)] = true;
            }

            public override T Rent<T>(Action<T> onGet) => default;
            public override void Return<T>(T obj) { }
        }

        [Test]
        public void Constructor_ShouldRegisterAllValidTypes()
        {
            // Arrange
            var typeRegister = Substitute.For<ITypeRegister>();
            var types = new Dictionary<int, Type>
            {
                { 1, typeof(ValidClass1) },
                { 2, typeof(ValidClass2) },
                { 3, typeof(InvalidClass) } // 没有无参构造函数
            };
            typeRegister.GetTypes().Returns(types);

            // Act
            var pool = new TestObjectPool(typeRegister);

            // Assert
            Assert.IsTrue(pool.RegisteredTypes.ContainsKey(typeof(ValidClass1)));
            Assert.IsTrue(pool.RegisteredTypes.ContainsKey(typeof(ValidClass2)));
            Assert.IsFalse(pool.RegisteredTypes.ContainsKey(typeof(InvalidClass)));
        }

        [Test]
        public void Constructor_ShouldPassCorrectDelegates()
        {
            // Arrange
            var typeRegister = Substitute.For<ITypeRegister>();
            typeRegister.GetTypes().Returns(new Dictionary<int, Type> { { 1, typeof(TestClass) } });

            Action<object> rentAction = obj => { };
            Action<object> returnAction = obj => { };
            Action<object> releaseAction = obj => { };

            // Act
            var pool = new TestObjectPool(
                typeRegister,
                _ => rentAction,
                _ => returnAction,
                _ => releaseAction);

            // Assert
            Assert.IsTrue(pool.RegisteredTypes.ContainsKey(typeof(TestClass)));
        }

        [Test]
        public void Constructor_ShouldHandleNullDelegates()
        {
            // Arrange
            var typeRegister = Substitute.For<ITypeRegister>();
            typeRegister.GetTypes().Returns(new Dictionary<int, Type> { { 1, typeof(TestClass) } });

            // Act
            var pool = new TestObjectPool(typeRegister);

            // Assert
            Assert.IsTrue(pool.RegisteredTypes.ContainsKey(typeof(TestClass)));
        }

        // 测试用的类定义
        public class ValidClass1 { }
        public class ValidClass2 { }
        public class InvalidClass { public InvalidClass(int arg) { } }
        public class TestClass { }
    }

    //public interface ITypeRegister
    //{
    //    Dictionary<int, Type> GetTypes();
    //}
}