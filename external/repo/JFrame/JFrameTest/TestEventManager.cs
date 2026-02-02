using JFramework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;

namespace JFrameTest
{
    public class TestEvent1 : Event
    {

    }
    internal class TestEventManager
    {
        [Test]
        public void TestRecieveEventCorrect()
        {
            
            //arrange
            var eventManager = new EventManager();
            bool called = false;
            eventManager.AddListener<TestEvent1>((e) => { called = true; });

            //action
            eventManager.Raise(eventManager.GetEvent<TestEvent1>());

            //expect
            Assert.AreEqual(true, called);
        }
    }
}
