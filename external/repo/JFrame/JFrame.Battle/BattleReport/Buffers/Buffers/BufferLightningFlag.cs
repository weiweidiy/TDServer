////using Newtonsoft.Json.Linq;
//using System.Collections.Generic;

//namespace JFrame
//{
//    /// <summary>
//    /// 每次普攻会给自己累计1点闪电标记，每层闪电标记提升自己5%的攻速。累计到6层消耗所有层数时召唤闪电攻击6次，每次造成100%的伤害。
//    /// 参数：[0]: 触发层数，[1]加速百分比
//    /// </summary>
//    public class BufferLightningFlag : Buffer
//    {
//        /// <summary>
//        /// 加速值
//        /// </summary>
//        float value;

//        ///// <summary>
//        ///// 当前触发值
//        ///// </summary>
//        //int curTriggeValue;

//        IBattleExecutor executor;

//        public BufferLightningFlag(IBattleUnit caster, bool isBuff, string UID, int id, int foldCount, float[] args, IBattleTrigger trigger, IBattleTargetFinder finder, List<IBattleExecutor> exutors) : base(caster,isBuff, UID, id, foldCount, args, trigger, finder, exutors)
//        {
//            if (args.Length < 2)
//                throw new System.Exception("BufferLightningFlag 参数不能少于2个");
//        }

//        //public override bool IsValid()
//        //{
//        //    return FoldCount < GetTriggeValue();
//        //}

//        /// <summary>
//        /// 触发值
//        /// </summary>
//        /// <returns></returns>
//        int GetTriggeValue()
//        {
//            return (int)Args[0];
//        }



//        /// <summary>
//        /// 需要提升攻速的百分比
//        /// </summary>
//        /// <returns></returns>
//        public virtual float GetValue()
//        {
//            return Args[1] * FoldCount;
//        }


//        /// <summary>
//        /// 计算CD
//        /// </summary>
//        /// <param name="originValue"></param>
//        /// <returns></returns>
//        protected virtual float CalcCD(float originValue)
//        {
//            return originValue / (1 + GetValue());
//        }


//        public override void OnAttach(IBattleUnit unit)
//        {
//            base.OnAttach(unit);

//            if (FoldCount >= GetTriggeValue())
//            {
//                //触发
//                NotifyOnCast(this);

//                //创建执行器
//                float[] arg = new float[4] { 3, 0, 0.2f, 1 };
//                executor = new ExecutorDamage(new FormulaManager(), arg);
//                executor.ReadyToExecute(Caster, null, new List<IBattleUnit>() { Owner });
//            }
//            else
//            {
//                //加攻速
//                var actions = Owner.GetActions();
//                foreach (var action in actions)
//                {
//                    if (action.Type == ActionType.Normal) //普通攻击
//                    {
//                        var cdTrigger = action.GetCDTrigger();
//                        var cdTimeTrigger = cdTrigger as CDTimeTrigger;
//                        if (cdTimeTrigger != null)
//                        {
//                            var args = cdTimeTrigger.GetArgs();
//                            var originValue = args[0];

//                            var cd = CalcCD(originValue);
//                            value = value + (originValue - cd);
//                            args[0] = cd;
//                            cdTimeTrigger.SetArgs(args);

//                            //Debug.LogError(target.Name + "OnAttach new cd " + cdTimeTrigger.GetArgs()[0]);
//                        }
//                    }
//                }
//            }
//        }

//        public override void OnDettach()
//        {
//            base.OnDettach();

//            var actions = Owner.GetActions();
//            foreach (var action in actions)
//            {
//                if (action.Type == ActionType.Normal) //普通攻击
//                {
//                    var cdTrigger = action.GetCDTrigger();
//                    var cdTimeTrigger = cdTrigger as CDTimeTrigger;
//                    if (cdTimeTrigger != null)
//                    {
//                        var args = cdTimeTrigger.GetArgs();
//                        args[0] += value;

//                        cdTimeTrigger.SetArgs(args);

//                        //Debug.LogError(target.Name + " OnDettach new cd " + cdTimeTrigger.GetArgs()[0]);
//                    }
//                }
//            }

//            //target.onActionCast -= Unit_onActionCast;
//            executor = null;
//        }




//        //private void Unit_onActionCast(IBattleUnit arg1, IBattleAction arg2, System.Collections.Generic.List<IBattleUnit> arg3, float arg4)
//        //{

//        //    curTriggeValue++;

//        //    if (curTriggeValue >= GetTriggeValue())
//        //    {
//        //        //触发
//        //        NotifyOnCast(this);

//        //        //创建执行器
//        //        float[] arg = new float[4] { 3,0,0.2f,1};
//        //        executor = new ExecutorDamage(new FormulaManager(), arg);
//        //        executor.ReadyToExecute(caster, null, new List<IBattleUnit>() { target });
//        //    }
//        //    else
//        //    {
//        //        //加攻速
//        //        var actions = target.GetActions();
//        //        foreach (var action in actions)
//        //        {
//        //            if (action.Type == ActionType.Normal) //普通攻击
//        //            {
//        //                var cdTrigger = action.GetCDTrigger();
//        //                var cdTimeTrigger = cdTrigger as CDTimeTrigger;
//        //                if (cdTimeTrigger != null)
//        //                {
//        //                    var args = cdTimeTrigger.GetArgs();
//        //                    var originValue = args[0];

//        //                    var cd = CalcCD(originValue);
//        //                    value = value + (originValue - cd);
//        //                    args[0] = cd;
//        //                    cdTimeTrigger.SetArgs(args);

//        //                    //Debug.LogError(target.Name + "OnAttach new cd " + cdTimeTrigger.GetArgs()[0]);
//        //                }
//        //            }
//        //        }
//        //    }
//        //}

//        public override void Update(BattleFrame frame)
//        {
//            base.Update(frame);

//            if(executor != null)
//                executor.Update(frame); 
//        }

//        public override float GetDuration()
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}


