using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 根據時間觸發  type = 3 參數0：時長
    /// </summary>
    public class TriggerTime : CombatBaseTrigger
    {

        float delta = 0f;

        public TriggerTime(List<CombatBaseFinder> finders) : base(finders)
        {
        }

        public override int GetValidArgsCount()
        {
            return 1;
        }

        public float GetDuration()
        {
            return GetCurArg(0);
        }

        public void SetDuration(float duration)
        {
            SetCurArg(0, duration);
        }

        public override void Reset()
        {
            base.Reset();

            delta = 0f;
        }

        protected override void OnUpdate(CombatFrame frame)
        {
            base.OnUpdate(frame);

            delta += frame.DeltaTime;

            if(delta >= GetDuration())
            {
                SetOn(true);

                //if(context != null && context.Logger != null )
                //{
                //    context.Logger.Log($"unitId:{ExtraData.Caster.GetUnitId()} cd: {GetDuration()}");
                //}
            }

        }

        public override void OnExitState()
        {
            base.OnExitState();

            ResetArgs();
        }
    }
}