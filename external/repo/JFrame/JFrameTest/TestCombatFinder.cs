//using JFrame.UI;
//using NUnit.Framework;
using NUnit.Framework;
using NSubstitute;
using JFramework;
using System.Collections.Generic;

namespace JFrameTest
{
    public class TestCombatFinder
    {
        CombatExtraData fakeExtraData;
        CombatUnit fakeUnit1;
        CombatUnit fakeUnit2;
        CombatUnit fakeUnit3;
        CombatContext fakeContext;
        CombatManager fakeManager;

        [SetUp]
        public void SetUp()
        {
            fakeExtraData = Substitute.For<CombatExtraData>();
            fakeUnit1 = Substitute.For<CombatUnit>();
            fakeUnit2 = Substitute.For<CombatUnit>();
            fakeUnit3 = Substitute.For<CombatUnit>();
            fakeContext = Substitute.For<CombatContext>();
            fakeManager = Substitute.For<CombatManager>();
            fakeContext.CombatManager.Returns(fakeManager);
            fakeExtraData.Owner.Returns(fakeUnit1);
        }


        [TearDown]
        public void Clear()
        {

        }

        [Test]
        public void TestFindSelf()
        {
            //arrange
            var findSelf = new FinderFindSelf();
            fakeExtraData.Owner.Returns(fakeUnit1);

            //act
            var targets = findSelf.FindTargets(fakeExtraData);


            //expect
            Assert.AreEqual(fakeUnit1, targets[0]);

        }

        [Test]
        public void TestFinderFindNearest()
        {
            //arrage
            var finder = new FinderFindNearest();
            finder.Initialize(fakeContext, new float[] { 1, 0, 0, 0, 2, 5f }); //找敌方5米内的1个单位

            fakeManager.GetOppoTeamId(fakeExtraData.Owner).Returns(1);
            fakeManager.GetUnits(1, true).Returns(new List<CombatUnit> { fakeUnit2, fakeUnit3 });
            fakeExtraData.Caster.Returns(fakeUnit1);
            fakeUnit1.GetPosition().Returns(new CombatVector() { x = 5 });
            fakeUnit2.GetPosition().Returns(new CombatVector() { x = 15 });
            fakeUnit3.GetPosition().Returns(new CombatVector() { x = 10 });
            fakeUnit1.IsAlive().Returns(true);
            fakeUnit2.IsAlive().Returns(true);
            fakeUnit3.IsAlive().Returns(true);

            //act 
            var result = finder.FindTargets(fakeExtraData);

            //expect
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(fakeUnit3, result[0]);
        }

        [Test]
        public void TestFinderFindNearestByMainType()
        {
            //arrage
            var finder = new FinderFindNearest();
            finder.Initialize(fakeContext, new float[] { 1, 0, 0, 0, 2, 5f }); //找敌方5米内的1个单位

            fakeManager.GetOppoTeamId(fakeExtraData.Owner).Returns(1);
            fakeManager.GetUnits(1, true).Returns(new List<CombatUnit> { fakeUnit2, fakeUnit3 });
            fakeExtraData.Caster.Returns(fakeUnit1);
            fakeUnit1.GetPosition().Returns(new CombatVector() { x = 5 });
            fakeUnit2.GetPosition().Returns(new CombatVector() { x = 15 });
            fakeUnit3.GetPosition().Returns(new CombatVector() { x = 10 });
            fakeUnit2.IsMainType(UnitMainType.Monster).Returns(true);
            fakeUnit3.IsMainType(UnitMainType.Monster).Returns(true);
            fakeUnit1.IsAlive().Returns(true);
            fakeUnit2.IsAlive().Returns(true);
            fakeUnit3.IsAlive().Returns(true);

            //act 
            var result = finder.FindTargets(fakeExtraData);

            //expect
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(fakeUnit3, result[0]);
        }


        [Test]
        public void TestFinderFindHpLessThanPercent()
        {
            //arrange
            var finder = new FinderFindHpLessThanPercent();
            finder.Initialize(fakeContext, new float[] { 0, 0, 0, 0, 2, 0.5f }); //找友军低于50%生命的单位
            fakeManager.GetFriendTeamId(fakeExtraData.Owner).Returns(0);
            fakeManager.GetUnits(0, true    ).Returns(new List<CombatUnit> { fakeUnit1, fakeUnit2, fakeUnit3 });
            fakeExtraData.Caster.Returns(fakeUnit1);
            fakeUnit1.GetHpPercent().Returns(0.8f);
            fakeUnit2.GetHpPercent().Returns(0.4f);
            fakeUnit3.GetHpPercent().Returns(0.1f);
            fakeUnit1.IsAlive().Returns(true);
            fakeUnit2.IsAlive().Returns(true);
            fakeUnit3.IsAlive().Returns(true);

            //act
            var result = finder.FindTargets(fakeExtraData);

            //expect
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(fakeUnit2, result[0]);
            Assert.AreEqual(fakeUnit3, result[1]);
        }

        [Test]
        public void TestFinderRangeByTarget()
        {
            //arrange
            var finder = new FinderFindRangeByTargets();
            finder.Initialize(fakeContext, new float[] { 0, 0, 0, 0, 3, 30f });
            fakeExtraData.Targets.Returns(new List<CombatUnit>() { fakeUnit1 });//取3个目标（unit1）左右15米的单位
            fakeExtraData.Caster.Returns(fakeUnit1);
            fakeManager.GetFriendTeamId(fakeExtraData.Targets[0]).Returns(1);
            fakeManager.GetUnits(1, true).Returns(new List<CombatUnit> { fakeUnit1, fakeUnit2, fakeUnit3 });
            fakeUnit1.GetPosition().Returns(new CombatVector() { x = 5 });
            fakeUnit2.GetPosition().Returns(new CombatVector() { x = 15 });
            fakeUnit3.GetPosition().Returns(new CombatVector() { x = 10 });
            fakeUnit1.IsAlive().Returns(true);
            fakeUnit2.IsAlive().Returns(true);
            fakeUnit3.IsAlive().Returns(true);

            //act
            var result = finder.FindTargets(fakeExtraData);

            //expect
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(fakeUnit1, result[0]);
        }

        [Test]
        public void TestFinderRangeByScreen()
        {
            //arrange
            var finder = new FinderFindRangeByScreen();
            finder.Initialize(fakeContext, new float[] { 0, 0, 0, 0, 3, 0, 15 });
            fakeExtraData.Targets.Returns(new List<CombatUnit>() { fakeUnit1 });
            fakeExtraData.Caster.Returns(fakeUnit1);
            fakeManager.GetFriendTeamId(fakeExtraData.Targets[0]).Returns(1);
            fakeManager.GetUnits(1, true).Returns(new List<CombatUnit> { fakeUnit1, fakeUnit2, fakeUnit3 });
            fakeUnit1.GetPosition().Returns(new CombatVector() { x = 5 });
            fakeUnit2.GetPosition().Returns(new CombatVector() { x = 15 });
            fakeUnit3.GetPosition().Returns(new CombatVector() { x = 10 });
            fakeUnit1.IsAlive().Returns(true);
            fakeUnit2.IsAlive().Returns(true);
            fakeUnit3.IsAlive().Returns(true);

            //act
            var result = finder.FindTargets(fakeExtraData);

            //expect
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(fakeUnit1, result[0]);
        }
    }


}
