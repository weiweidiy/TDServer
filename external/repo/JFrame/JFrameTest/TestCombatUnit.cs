//using JFrame.UI;
//using NUnit.Framework;
using JFramework;
using NSubstitute;
using NUnit.Framework;

namespace JFrameTest
{
    public class TestCombatUnit
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
        public void TestUnitMove()
        {
            //arrage
            var unit = new CombatUnit();
            var attributeManager = NSubstitute.Substitute.For<CombatAttributeManger>();
            attributeManager.Get(Arg.Any<string>()).Returns(new CombatAttributeDouble(CombatAttribute.ATK.ToString(), 10, 10)); //默认会调用atk
            unit.Initialize(new CombatUnitInfo() { uid = "uid" }, null, null, null, attributeManager);
            unit.SetPosition(new CombatVector() { x = 10, y = 0 });
            unit.SetSpeed(new CombatVector() { x = -1, y = 0 });
            unit.SetTargetPosition(new CombatVector() { x = 0, y = 0 });

            //act
            unit.StartMove();
            unit.UpdatePosition(new CombatFrame());
            unit.Update(new CombatFrame());

            //expect
            Assert.AreEqual(9, unit.GetPosition().x);
        }


    }


}
