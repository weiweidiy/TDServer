using System;

namespace JFramework
{

    public class JFrameTimer : ITimer
    {
        int id;
        PETimer pt;
        public JFrameTimer(PETimer pt, int id)
        {
            this.pt = pt;
            this.id = id;
        }
        public void Stop()
        {
            //定时任务删除
            pt.DeleteTimeTask(id);
        }
    }
    public class JFrameTimerUtils : ITimerUtils
    {
        PETimer pt = new PETimer();
        Action action;
        public ITimer Regist(float interval, int loopTimes, Action action, bool immediatly = false, bool useRealTime = false)
        {
            this.action = action; 

            int count = loopTimes == -1? 0 : loopTimes;

            int tempID = pt.AddTimeTask((int tid) => {
                action?.Invoke();
            }, interval, PETimeUnit.Second, count);

            return new JFrameTimer(pt, tempID);
        }

        /// <summary>
        /// 手动调用
        /// </summary>
        public void Call()
        {
            action?.Invoke();
        }

        public void Update()
        {
            pt.Update();
        }
    }
}


