//using JFrame.UI;
//using NUnit.Framework;
using JFramework;
using NUnit.Framework;
using System.Collections.Generic;
using NSubstitute;

namespace JFrameTest
{
    public class TestCombatManager
    {
        SingleCombatManager combatManager;

        List<CombatUnitInfo> team1;
        List<CombatUnitInfo> team2;
 

        [SetUp]
        public void SetUp()
        {
            combatManager = new SingleCombatManager();
            team1 = new List<CombatUnitInfo>();
            team2 = new List<CombatUnitInfo>();
            
            team1.Add(CreateUnitInfo("1",100,10,1f, new CombatVector(), new CombatVector()));
            team2.Add(CreateUnitInfo("2", 1000, 1, 1f, new CombatVector() { x = 5 }, new CombatVector() { x = -1f }));
            team2.Add(CreateUnitInfo("3", 2000, 1, 1f, new CombatVector() { x = 6 }, new CombatVector() { x = -1f }));

        }

        CombatUnitInfo CreateUnitInfo(string uid, int hp, int atk, float atkSpeed, CombatVector position, CombatVector moveSpeed)
        {
            var unitInfo = new CombatUnitInfo();
            unitInfo.uid = "1";
            unitInfo.hp = 100;
            unitInfo.atk = 10;
            unitInfo.atkSpeed = 1f;
            unitInfo.position = new CombatVector() { x = 0, y = 0 };
            unitInfo.moveSpeed = new CombatVector() { x = 0, y = 0 };
            return unitInfo;
        }

        [TearDown]
        public void Clear()
        {
            
        }


        [Test]
        public void TestCombat()
        {
            //arrange
            var dicTeam1 = new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, team1);
            var dicTeam2 = new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, team2);
            combatManager.Initialize(dicTeam1, dicTeam2, new List<CombatBufferInfo>(), 90);

            //act
            //combatManager.StartUpdate();

            //expect
        }


        [Test]
        public void TestCombatManagerInitTeam()
        {
            //arrange
            var unitInfos = Substitute.For<List<CombatUnitInfo>>();
            unitInfos.Add(new CombatUnitInfo());

            //act
            var dicTeam1 = new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, unitInfos);
            var dicTeam2 = new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, new List<CombatUnitInfo>());
            combatManager.Initialize(dicTeam1, dicTeam2, new List<CombatBufferInfo>(), 90);

            //expect
            Assert.AreEqual(1, combatManager.GetTeams().Count);
        }

        [Test]
        public void TestCombatManagerInitTeamAndUnit()
        {
            //arrange
            var team1Info = new List<CombatUnitInfo>();
            team1Info.Add(new CombatUnitInfo());
            var team2Info = new List<CombatUnitInfo>();
            team2Info.Add(new CombatUnitInfo());

            //act
            var dicTeam1 = new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, team1Info);
            var dicTeam2 = new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, team2Info);
            combatManager.Initialize(dicTeam1, dicTeam2, new List<CombatBufferInfo>(), 90);

            //expect
            Assert.AreEqual(1, combatManager.GetUnitCount(0));
            Assert.AreEqual(1, combatManager.GetUnitCount(1));
        }

        [Test]
        public void TestCombatManagerGetUnitInRange()
        {
            //arrange 
            var lstCombat = new List<CombatUnit>();
            var unit1 = Substitute.For<CombatUnit>();
            unit1.GetPosition().Returns(new CombatVector() { x = 1 });
            unit1.IsAlive().Returns(true);
            var unit2 = Substitute.For<CombatUnit>();
            unit2.GetPosition().Returns(new CombatVector() { x = 2 });
            unit2.IsAlive().Returns(true);
            var myUnit = Substitute.For<CombatUnit>();
            myUnit.IsAlive().Returns(true);
            myUnit.GetPosition().Returns(new CombatVector() { x = 0 });
            lstCombat.Add(unit1);
            lstCombat.Add(unit2);
            var team1 = Substitute.For<CommonCombatTeam>();
            team1.GetUnits().Returns(lstCombat);
            var dicTeam1 = new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, new List<CombatUnitInfo>());
            var dicTeam2 = new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, new List<CombatUnitInfo>());
            combatManager.Initialize(dicTeam1, dicTeam2, new List<CombatBufferInfo>(), 90);
            combatManager.AddTeam(1, team1);

            //act
            var units = combatManager.GetUnitsInRange(myUnit, 1, 2f);

            //expect
            Assert.AreEqual(2,units.Count);
        }


        [Test]
        public void TestCombatResult()
        {
            //arrange
            var combatManager = new SingleCombatManager();
            //combatManager.Initialize()

            //act

            //expect
        }

    }


}
