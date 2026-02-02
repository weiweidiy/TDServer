using System;
using System.Collections.Generic;

namespace JFramework
{

    /// <summary>
    /// 搜索器基类
    /// </summary>
    public abstract class BaseTargetFinder : IBattleTargetFinder
    {
        protected BattlePoint selfPoint;
        protected IPVPBattleManager manger;
        protected float arg;

        public BaseTargetFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg)
        {
            this.selfPoint = selfPoint;
            this.manger = manger;
            this.arg = arg;
        }

        public IAttachOwner Owner { get; private set; }

        public abstract List<IBattleUnit> FindTargets(object[] args);

        public float[] GetArgs()
        {
            throw new NotImplementedException();
        }

        public void OnAttach(IAttachOwner target)
        {
            Owner = target;
        }

        public void OnDetach()
        {
            //throw new System.NotImplementedException();
        }

        public void SetArgs(float[] args)
        {
            if (args.Length == 0)
                throw new Exception("basefinder 设置参数不正确");

            arg = args[0];
        }
    }

}

///// <summary>
///// 搜索器基类
///// </summary>
//public abstract class BaseTargetFinder : IBattleTargetFinder
//{
//    protected BattlePoint selfPoint;
//    protected IPVPBattleManager manger;
//    protected float arg;

//    public BaseTargetFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg)
//    {
//        this.selfPoint = selfPoint;
//        this.manger = manger;
//        this.arg = arg;
//    }

//    public IBattleAction Owner { get; private set; }

//    public abstract List<IBattleUnit> FindTargets();

//    public void OnAttach(IBattleAction action)
//    {
//        Owner = action;
//    }
//}
