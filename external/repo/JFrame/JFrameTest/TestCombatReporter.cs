//using JFrame.UI;
//using NUnit.Framework;
using JFramework;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace JFrameTest
{
    public class TestCombatReporter
    {
        [SetUp]
        public void SetUp()
        {
            //combatManager = new CombatManager();
        }


        [TearDown]
        public void Clear()
        {

        }

        [Test]
        public void TestUnitAddBuffer()
        {
            //arrange
            var fakeTeam = Substitute.For<CommonCombatTeam>();
            var fakeContext = Substitute.For<CombatContext>();
            var unitsInfo = new List<CombatUnitInfo>();
            var fakeUnit = Substitute.For<CombatUnit>();
            fakeUnit.onBufferAdded += (extraData) => {  };
            fakeTeam.Initialize(0, fakeContext, unitsInfo);
            fakeTeam.CreateUnits(fakeContext, unitsInfo).Returns(new List<CombatUnit>() { fakeUnit });

            var reporter = new CombatReporter(new CombatFrame(), new List<CommonCombatTeam>() { });

            //act
            //fakeUnit.onBufferAdded += Raise.Event<Action<CombatExtraData>>(null);

            //expect
        }
    }


}
