using JFramework;
using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;
using System;


namespace JFrameTest
{
    public class TestCombatBufferManager
    {
        BaseCombatBuffer buffer1;
        BaseCombatBuffer buffer2;

        [SetUp]
        public void Setup()
        {
            buffer1 = Substitute.For<BaseCombatBuffer>();
            buffer2 = Substitute.For<BaseCombatBuffer>();
        }

        [Test]
        public void TestAddBufferUnion()
        {
            //arrage
            buffer1.FoldType.Returns(CombatBufferFoldType.Union);
            buffer2.FoldType.Returns(CombatBufferFoldType.Union);

            var manager = new CombatBufferManager();

            //act
            manager.AddItem(buffer1);
            manager.Update(new CombatFrame());
            manager.AddItem(buffer2);
            manager.Update(new CombatFrame());

            //expect
            Assert.AreEqual(2, manager.GetAll().Count);
        }

        [Test]
        public void TestAddBufferReplace()
        {
            //arrage
            buffer1.FoldType.Returns(CombatBufferFoldType.Replace);
            buffer1.Id.Returns(1);
            buffer1.Uid.Returns("buffer1");
            buffer2.FoldType.Returns(CombatBufferFoldType.Replace);
            buffer2.Id.Returns(1);
            buffer2.Uid.Returns("buffer2");

            var manager = new CombatBufferManager();

            //act
            manager.AddItem(buffer1);
            manager.Update(new CombatFrame());
            manager.AddItem(buffer2);
            manager.Update(new CombatFrame());


            //expect
            Assert.AreEqual(1, manager.GetAll().Count);
            Assert.AreEqual("buffer2", manager.GetAll()[0].Uid);
        }

        [Test]
        public void TestAddBufferFold()
        {
            //arrage
            buffer1.FoldType.Returns(CombatBufferFoldType.Fold);
            buffer1.Id.Returns(1);
            buffer1.GetCurFoldCount().Returns(1);
            buffer1.Uid.Returns("buffer1");
            buffer2.FoldType.Returns(CombatBufferFoldType.Fold);
            buffer2.Id.Returns(1);
            buffer2.Uid.Returns("buffer2");
            buffer2.GetCurFoldCount().Returns(1);


            var manager = new CombatBufferManager();

            //act
            manager.AddItem(buffer1);
            manager.Update(new CombatFrame());
            manager.AddItem(buffer2);
            manager.Update(new CombatFrame());


            //expect
            Assert.AreEqual(1, manager.GetAll().Count);
            Assert.AreEqual("buffer1", manager.GetAll()[0].Uid);
            buffer2.Received(1).SetCurFoldCount(2);

        }

        [Test]
        public void TestBufferExpired()
        {
            //arrage
            buffer1.FoldType.Returns(CombatBufferFoldType.Union);
            buffer1.Expired.Returns(true);
            var manager = new CombatBufferManager();

            //act
            manager.AddItem(buffer1);
            manager.Update(new CombatFrame());
            manager.Update(new CombatFrame());

            //expect
            Assert.AreEqual(0, manager.GetAll().Count);
        }
    }


}
