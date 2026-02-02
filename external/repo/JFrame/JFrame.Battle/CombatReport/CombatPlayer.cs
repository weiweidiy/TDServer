

using System;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace JFramework
{

    public abstract class CombatPlayer
    {
        /// <summary>
        /// 开始播放
        /// </summary>
        public event Action onPlayerStart;

        /// <summary>
        /// 退出播放
        /// </summary>
        public event Action<bool> onPlayerExit;
        public void NotifyExit(bool win) => onPlayerExit?.Invoke(win);

        /// <summary>
        /// 缩放变更了
        /// </summary>
        public event Action<float> onScaleChanged;

        /// <summary>
        /// 战报结果
        /// </summary>
        protected CombatReport report;

        /// <summary>
        /// 战报解析器
        /// </summary>
        protected CombatReprotParser parser;

        /// <summary>
        /// 播放速度
        /// </summary>
        protected float playScale = 1f;

        /// <summary>
        /// 加载战报
        /// </summary>
        /// <param name="report"></param>
        public virtual async Task LoadReport(CombatReport report)
        {
            this.report = report;
            parser = new CombatReprotParser(report.report);
            await parser.LoadData(report.report);
        }

        public virtual void Release() { }


        bool isPlaying;

        bool isPaused;
        /// <summary>
        /// 流逝的总时间
        /// </summary>
        float escapeTime = 0f;


        /// <summary>
        /// 播放
        /// </summary>
        public virtual void Play()
        {
            escapeTime = 0f;
            isPlaying = true;
            isPaused = false;
            //UnityEngine.Debug.LogError("开始播放 " + GetHashCode());
            onPlayerStart?.Invoke();
        }

        /// <summary>
        /// 设置播放速度
        /// </summary>
        /// <param name="scale"></param>
        public virtual void SetPlayScale(float scale)
        {
            if(playScale != scale)
            {
                playScale = scale;
                isPaused = scale == 0f;
                onScaleChanged?.Invoke(playScale);

                //Debug.LogError("playscale " + playScale);
            }
        }

        /// <summary>
        /// 获取当前缩放
        /// </summary>
        /// <returns></returns>
        public float GetPlayScale() => playScale;


        public virtual void Stop()
        {
            //escapeTime = 0f;
            isPlaying = false;
            //UnityEngine.Debug.LogError("播放完毕 " + GetHashCode());
        }

        /// <summary>
        /// 跳过
        /// </summary>
        public virtual void Skip()
        {
            escapeTime = 100;
        }

        /// <summary>
        /// 获取总的流逝时间
        /// </summary>
        /// <returns></returns>
        public float GetEscapeTime() => escapeTime;

        /// <summary>
        /// 重播
        /// </summary>
        public abstract void Replay();

        public abstract float GetDeltaTime();


        float delta = 0;
        public void Update()
        {
            if (!isPlaying || isPaused)
                return;

            escapeTime += (GetDeltaTime() /** playScale*/);

            //获取需要播放的数据
            var lstData = parser.GetData(escapeTime);
            if (lstData.Count > 0)
            {
                //UnityEngine.Debug.Log("开始播放 ");
                foreach (var data in lstData)
                {
                    //Debug.Log("play " + data.ReportType + " frame " + escapeTime);
                    PlayData(data);
                }

               // Debug.LogError("escape " + escapeTime);
            }

            else if (parser.Count() == 0)
            {
                delta += GetDeltaTime() /** playScale*/;

                if (delta < 1f)
                    return;

                delta = 0;

                Stop();
                //Debug.LogError("OnReportEnd " + escapeTime);
                //战报结束了
                OnReportEnd(report.winner == 1);
            }
        }

        protected virtual void OnReportEnd(bool result)
        {

        }

        void PlayData(ICombatReportData data)
        {
            var actionName = data.ReportType;
            switch (actionName)
            {
                case ReportType.StartMove:
                    {
                        PlayStartMove(data);
                    }
                    break;
                case ReportType.EndMove:
                    {
                        PlayEndMove(data);
                    }
                    break;
                case ReportType.ActionCast:
                    {
                        PlayAction(data);
                    }
                    break;
                case ReportType.Damage:
                    {
                        PlayDamage(data);
                    }
                    break;
                case ReportType.Dead:
                    {
                        PlayDead(data);
                    }
                    break;
                case ReportType.AddBuffer:
                    {
                        PlayAddBuffer(data);
                        // Debug.LogError("AddBuffer");
                    }
                    break;
                case ReportType.Heal:
                    {
                        PlayHeal(data);
                    }
                    break;
                case ReportType.RemoveBuffer:
                    {
                        PlayRemoveBuffer(data);
                    }
                    break;
                case ReportType.Reborn:
                    {
                        PlayReborn(data);
                    }
                    break;
                case ReportType.MaxHpUp:
                    {
                        //Debug.LogError("MaxHpUp" + (int)data.Arg[2]);
                        //PlayHeal(data);
                    }
                    break;
                case ReportType.ActionCD:
                    {
                        PlayActionCD(data);
                    }
                    break;
 
                case ReportType.DebuffAnti:
                    {
                        //PlayDebuffAnti(data);
                    }
                    break;
                case ReportType.UpdateBuffer:
                    {

                    }
                    break;
                case ReportType.ShootChange:
                    {
                        PlayShootChange(data);
                    }
                    break;
                case ReportType.Miss:
                    {
                        PlayMiss(data);
                    }
                    break;
                default:
                    throw new System.Exception("没有实现的动作类型" + actionName);
            }
        }

        protected abstract void PlayShootChange(ICombatReportData data);
        protected abstract void PlayActionCD(ICombatReportData data);
        protected abstract void PlayReborn(ICombatReportData data);
        protected abstract void PlayRemoveBuffer(ICombatReportData data);
        protected abstract void PlayAddBuffer(ICombatReportData data);

        protected abstract void PlayStartMove(ICombatReportData data);

        protected abstract void PlayEndMove(ICombatReportData data);

        /// <summary>
        /// 播放攻击动作
        /// </summary>
        /// <param name="data"></param>
        protected abstract void PlayAction(ICombatReportData data);

        /// <summary>
        /// 播放各种掉血，加血，添加BUFF，移除BUFF，复活等
        /// </summary>
        /// <param name="data"></param>
        protected abstract void PlayDamage(ICombatReportData data);


        protected abstract void PlayMiss(ICombatReportData data);


        /// <summary>
        /// 播放死亡
        /// </summary>
        /// <param name="data"></param>
        protected abstract void PlayDead(ICombatReportData data);


        /// <summary>
        /// 播放加血
        /// </summary>
        /// <param name="data"></param>
        protected abstract void PlayHeal(ICombatReportData data);

        /// <summary>
        /// 播放结果
        /// </summary>
        /// <param name="win"></param>
        protected abstract void PlayResult(int win);
    }
}
