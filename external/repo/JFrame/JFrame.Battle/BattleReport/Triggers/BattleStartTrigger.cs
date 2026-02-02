namespace JFramework
{



    /// <summary>
    /// 战斗开始触发，只触发1次  arg: 没用 type = 3
    /// </summary>
    public class BattleStartTrigger : BaseBattleTrigger
    {
        public BattleStartTrigger(IPVPBattleManager battleManager, float[] arg, float delay = 0) : base(battleManager, arg, delay)
        {

        }



        protected override void OnDelayCompleteEveryFrame(CombatFrame frame)
        {
            base.OnDelayCompleteEveryFrame(frame);

            SetOn(true);
        }
    }
}

///// <summary>
///// 战斗开始触发，只触发1次  arg: 没用 type = 3
///// </summary>
//public class BattleStartTrigger : BaseBattleTrigger
//{
//    public BattleStartTrigger(IPVPBattleManager battleManager, float[] arg, float delay = 0) : base(battleManager, arg, delay)
//    {

//    }



//    protected override void OnDelayCompleteEveryFrame()
//    {
//        base.OnDelayCompleteEveryFrame();

//        SetOn(true);
//    }
//}