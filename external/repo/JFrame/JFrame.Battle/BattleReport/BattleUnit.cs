using JFramework.BattleReportSystem;
using System;
using System.Collections.Generic;
using System.Linq;


namespace JFramework
{
    public class BattleUnit : IBattleUnit
    {
        /// <summary>
        /// 有动作准备完毕，可以释放了
        /// </summary>
        //主体事件
        public event Action<IBattleUnit, IBattleAction, List<IBattleUnit>, float> onActionCast;
        public event Action<IBattleUnit, IBattleAction, float> onActionStartCD;
        public event Action<IBattleUnit, IBattleAction, IBattleUnit, ExecuteInfo> onHittingTarget; //动作命中对方

        //受体事件
        public event Action<IBattleUnit, IBattleAction, IBattleUnit, ExecuteInfo> onDamaging; //即将受到伤害
        public event Action<IBattleUnit, IBattleAction, IBattleUnit, ExecuteInfo> onDamaged;
        public event Action<IBattleUnit, IBattleAction, IBattleUnit, int> onHealed;        //回血
        public event Action<IBattleUnit, IBattleAction, IBattleUnit> onDead;
        public event Action<IBattleUnit, IBattleAction, IBattleUnit, int> onRebord;        //复活
        public event Action<IBattleUnit, IBattleAction, IBattleUnit, int> onMaxHpUp;       //复活
        public event Action<IBattleUnit, IBattleAction, IBattleUnit, int> onDebuffAnti;    //状态抵抗

        public event Action<IBattleUnit, int, ExecuteInfo> onBufferAdding; //即将添加buff
        public event Action<IBattleUnit, IBuffer> onBufferAdded;
        public event Action<IBattleUnit, IBuffer> onBufferRemoved;
        public event Action<IBattleUnit, IBuffer> onBufferCast;
        public event Action<IBattleUnit, IBuffer, int, float[]> onBufferUpdate;

        /// <summary>
        /// 获取战斗对象名字，暂时用ID代替
        /// </summary>
        public string Name => battleUnitInfo.id.ToString();

        /// <summary>
        /// 唯一ID
        /// </summary>
        public string Uid { get; set; }



        /// <summary>
        /// 所有动作列表
        /// </summary>
        IActionManager actionManager = null;

        /// <summary>
        /// 战斗单位原始数据
        /// </summary>
        BattleUnitInfo battleUnitInfo = default;

        /// <summary>
        /// 战斗单位属性
        /// </summary>
        BattleUnitAttribute battleUnitAttribute = default;

        /// <summary>
        /// buff管理器
        /// </summary>
        IBufferManager bufferManager = null;



        public BattleUnit(BattleUnitInfo info, IActionManager actionManager, IBufferManager bufferManager)
        {
            this.Uid = info.uid;
            battleUnitInfo = info;


            Atk = info.atk;
            MaxHP = info.maxHp;
            HP = info.hp;
            AtkSpeed = info.atkSpeed;
            Critical = info.cri;
            CriticalDamage = info.criDmgRate;
            CriticalDamageResist = info.criDmgAnti;
            SkillDamageEnhance = info.skillDmgRate;
            SkillDamageReduce = info.skillDmgAnti;
            DamageEnhance = info.dmgRate;
            DamageReduce = info.dmgAnti;
            ControlHit = info.debuffHit;
            ControlResistance = info.debuffAnti;
            Puncture = info.penetrate;
            Block = info.block;


            this.bufferManager = bufferManager;
            if (this.bufferManager != null)
            {
                this.bufferManager.onBufferAdded += BufferManager_onBufferAdded;
                this.bufferManager.onBufferRemoved += BufferManager_onBufferRemoved;
                this.bufferManager.onBufferCast += BufferManager_onBufferCast;
                this.bufferManager.onBufferUpdated += BufferManager_onBufferUpdated;
                this.bufferManager.Owner = this;
            }

            this.actionManager = actionManager;
        }

        public void Initialize()
        {
            if (actionManager != null)
            {
                actionManager.Initialize(this);
                actionManager.onStartCast += Action_onCast;
                actionManager.onStartCD += ActionManager_onStartCD;
                actionManager.onHittingTarget += ActionManager_onHittingTarget;
            }
        }



        /// <summary>
        /// 更新帧了
        /// </summary>
        /// <param name="frame"></param>
        public void Update(CombatFrame frame)
        {
            actionManager.Update(frame);

            bufferManager.Update(frame);
        }

        /// <summary>
        /// 是否活着
        /// </summary>
        /// <returns></returns>
        public bool IsAlive()
        {
            return HP > 0;
        }

        /// <summary>
        /// 是否是增益
        /// </summary>
        /// <param name="bufferId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsBuffer(int bufferId)
        {
            return bufferManager.IsBuff(bufferId);
        }

        /// <summary>
        /// 是否满血
        /// </summary>
        /// <returns></returns>
        public bool IsHpFull()
        {
            return HP == MaxHP;
        }

        public IBattleAction[] GetActions()
        {
            return actionManager.GetAll();
        }

        public IBattleAction[] GetActions(ActionType type)
        {
            return actionManager.GetActionsByType(type).ToArray();
        }

        public IBattleAction GetAction(int actionId)
        {
            return actionManager.GetAction(actionId);
        }

        #region 响应事件
        /// <summary>
        /// buffer添加了
        /// </summary>
        /// <param name="obj"></param>
        private void BufferManager_onBufferAdded(IBuffer obj)
        {
            onBufferAdded?.Invoke(this, obj);
        }

        /// <summary>
        /// buffer触发了
        /// </summary>
        /// <param name="obj"></param>
        private void BufferManager_onBufferCast(IBuffer obj)
        {
            onBufferCast?.Invoke(this, obj);
        }

        /// <summary>
        /// buffer移除了
        /// </summary>
        /// <param name="obj"></param>
        private void BufferManager_onBufferRemoved(IBuffer obj)
        {
            onBufferRemoved?.Invoke(this, obj);
        }

        /// <summary>
        /// buffer更新了
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void BufferManager_onBufferUpdated(IBuffer arg1, int arg2, float[] arg3)
        {
            onBufferUpdate?.Invoke(this, arg1, arg2, arg3);
        }

        /// <summary>
        /// 发动
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void Action_onCast(IBattleAction action, List<IBattleUnit> targets, float duration)
        {
            onActionCast?.Invoke(this, action,targets, duration);
        }

        /// <summary>
        /// 进入CD了
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cd"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ActionManager_onStartCD(IBattleAction action, float cd)
        {
            onActionStartCD?.Invoke(this, action, cd);
        }

        /// <summary>
        /// 即将命中目标
        /// </summary>
        /// <param name="action"></param>
        /// <param name="target"></param>
        /// <param name="info"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ActionManager_onHittingTarget(IBattleAction action, IBattleUnit target, ExecuteInfo info)
        {
            onHittingTarget?.Invoke(this, action, target,info);
        }

        /// <summary>
        /// 受到了伤害
        /// </summary>
        /// <param name="damage"></param>
        public void OnDamage(IBattleUnit hitter, IBattleAction action, ExecuteInfo damage)
        {
            //to do: 添加一个预伤害事件，可以修改值
            onDamaging?.Invoke(hitter, action,this, damage);

            if (HP <= 0 || damage.Value == 0)
                return;

            HP -= damage.Value;

            onDamaged?.Invoke(hitter, action, this, damage);

            if (HP <= 0)
            {
                OnDead(hitter, action);           
            }
        }

        /// <summary>
        /// 受到治疗
        /// </summary>
        /// <param name="heal"></param>
        public void OnHeal(IBattleUnit caster, IBattleAction action, ExecuteInfo heal)
        {
            //to do: 添加一个预治疗事件，可以修改值

            if (!IsAlive())
                return;

            HP += heal.Value;

            onHealed?.Invoke(caster, action, this, heal.Value);

        }

        /// <summary>
        /// 生命上限提高
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="action"></param>
        /// <param name="hp"></param>
        public void OnMaxHpUp(IBattleUnit caster, IBattleAction action, ExecuteInfo hp)
        {

            if (!IsAlive())
                return;

            MaxHPUpgrade(hp.Value);

            onMaxHpUp?.Invoke(caster,action, this, hp.Value);
        }

        /// <summary>
        /// 复活
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="action"></param>
        /// <param name="heal"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnReborn(IBattleUnit caster, IBattleAction action, ExecuteInfo heal)
        {

            if (IsAlive())
                return;

            HP += heal.Value;

            //to do: 移到actionmanager中
            if (action != null)
            {
                foreach (var a in actionManager.GetAll())
                {
                    a.SetDead(false);
                }
            }

            onRebord?.Invoke(caster, action, this, heal.Value);
        }

        /// <summary>
        /// 死亡了
        /// </summary>
        private void OnDead(IBattleUnit hitter, IBattleAction action)
        {
            if(actionManager != null)
                actionManager.OnDead();

            if (bufferManager != null)
                bufferManager.Clear();

            onDead?.Invoke(hitter, action, this);
        }

        /// <summary>
        /// 状态抵抗了
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="action"></param>
        /// <param name="info"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnDebuffAnti(IBattleUnit caster, IBattleAction action,  int debuffId)
        {
            onDebuffAnti?.Invoke(caster,action,this, debuffId);
        }

        /// <summary>
        /// 眩晕了
        /// </summary>
        /// <param name="duration"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnStunning(ActionType actionType, float duration)
        {
            if(actionManager != null)
                actionManager.OnStunning(actionType, duration);
        }

        /// <summary>
        /// 眩晕恢复
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void OnResumeFromStunning(ActionType actionType)
        {
            if (actionManager != null)
                actionManager.OnResumeFromStunning(actionType);
        }

        #endregion

        #region buff
        /// <summary>
        /// 添加buffer
        /// </summary>
        /// <param name="bufferId"></param>
        /// <param name="foldCout"></param>
        /// <returns></returns>
        public IBuffer AddBuffer(IBattleUnit caster, int bufferId, int foldCout = 1)
        {
            if (bufferManager == null)
                throw new Exception("没有设置bufferManager 不能AddBuffer " + Name);

            var info = new ExecuteInfo() { IsImmunity = false };
            onBufferAdding?.Invoke(this, bufferId, info);

            if(!info.IsImmunity)
            {
                return bufferManager.AddBuffer(caster, this, bufferId, foldCout);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取所有buffers
        /// </summary>
        /// <returns></returns>
        public IBuffer[] GetBuffers()
        {
            return bufferManager.GetBuffers();
        }

        /// <summary>
        /// 移除buffer
        /// </summary>
        /// <param name="bufferUID"></param>
        public void RemoveBuffer(string bufferUID)
        {
            bufferManager.RemoveBuffer(bufferUID);
        }

        #endregion

        #region 属性


        public int Atk
        {
            get { return battleUnitAttribute.atk; }
            private set { battleUnitAttribute.atk = Math.Max(0, value); }
        }

        /// <summary>
        /// 攻击提升
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int AtkUpgrade(int value)
        {
            //if (value < 0)
            //    throw new Exception("攻击提升数值不能为负数 " + value);

            if (value < 0)
            {
                //Debug.LogError("AtkUpgrade 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            Atk += value;
            return value;
        }

        /// <summary>
        /// 攻击力降低
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int AtkReduce(int value)
        {
            //if (value < 0)
            //    throw new Exception("攻击降低数值不能为负数 " + value);

            if (value < 0)
            {
                //Debug.LogError("AtkReduce 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            var realValue = Math.Min(value, Atk); //防止减成负数
            Atk -= realValue;
            return realValue;
        }

        /// <summary>
        /// 攻击速度
        /// </summary>
        public float AtkSpeed
        {
            get { return battleUnitAttribute.atkSpeed; }
            set { battleUnitAttribute.atkSpeed = Math.Max(0, value); }
        }

        /// <summary>
        /// 生命值
        /// </summary>
        public int HP
        {
            get { return battleUnitAttribute.hp; }
            private set
            {
                battleUnitAttribute.hp = Math.Min(MaxHP, Math.Max(0, value));
            }
        }

        /// <summary>
        /// 最大生命值
        /// </summary>
        public int MaxHP
        {
            get => battleUnitAttribute.maxHp;
            private set
            {
                battleUnitAttribute.maxHp = value;
                if (HP > value)
                    HP = value;
            }
        }

        /// <summary>
        /// 最大生命升级
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int MaxHPUpgrade(int value)
        {
            //if (value < 0)
            //    throw new Exception("最大生命提升数值不能为负数 " + value);

            if (value < 0)
            {
                //Debug.LogError("MaxHPUpgrade 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            MaxHP += value;
            HP += value; //当前生命也要上升
            return value;
        }

        /// <summary>
        /// 最大生命值降低
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int MaxHPReduce(int value)
        {
            //if (value < 0)
            //    throw new Exception("MaxHp降低数值不能为负数 " + value);

            if (value < 0)
            {
                //Debug.LogError("MaxHPReduce 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            var realValue = Math.Min(value, MaxHP); //防止减成负数
            MaxHP -= realValue;
            return realValue;
        }


        public float Critical
        {
            get { return battleUnitAttribute.cri; }
            private set
            {
                battleUnitAttribute.cri = value;
            }
        }

        public float CriUpgrade(float value)
        {
            //if (value < 0)
            //    throw new Exception("暴击提升数值不能为负数 " + value);

            if (value < 0)
            {
                //Debug.LogError("CriUpgrade 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            Critical += value;
            return value;
        }

        public float CriReduce(float value)
        {
            //if (value < 0)
            //    throw new Exception("暴击降低数值不能为负数 " + value);

            if (value < 0)
            {
                //Debug.LogError("CriReduce 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            var realValue = Math.Min(value, Critical); //防止减成负数
            Critical -= realValue;
            return realValue;
        }

        public float CriticalDamage
        {
            get { return battleUnitAttribute.criDmgRate; }
            private set
            {
                battleUnitAttribute.criDmgRate = value;
            }
        }

        public float CriticalDamageUpgrade(float value)
        {
            //if (value < 0)
            //    throw new Exception("CriticalDamage提升数值不能为负数 " + value);

            if (value < 0)
            {
                //Debug.LogError("CriticalDamageUpgrade 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            CriticalDamage += value;
            return value;
        }

        public float CriticalDamageReduce(float value)
        {
            //if (value < 0)
            //    throw new Exception("CriticalDamage降低数值不能为负数 " + value);

            if (value < 0)
            {
               // Debug.LogError("CriticalDamageReduce 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            var realValue = Math.Min(value, CriticalDamage); //防止减成负数
            CriticalDamage -= realValue;
            return realValue;
        }

        public float CriticalDamageResist
        {
            get { return battleUnitAttribute.criDmgAnti; }
            private set
            {
                battleUnitAttribute.criDmgAnti = value;
            }
        }

        public float CriticalDamageResistUpgrade(float value)
        {
            //if (value < 0)
            //    throw new Exception("CriticalDamageResist提升数值不能为负数 " + value);

            if (value < 0)
            {
                //Debug.LogError("CriticalDamageResistUpgrade 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            CriticalDamageResist += value;
            return value;
        }

        public float CriticalDamageResistReduce(float value)
        {
            //if (value < 0)
            //    throw new Exception("CriticalDamageResist降低数值不能为负数 " + value);

            if (value < 0)
            {
               // Debug.LogError("CriticalDamageResistReduce 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            var realValue = Math.Min(value, CriticalDamageResist); //防止减成负数
            CriticalDamageResist -= realValue;
            return realValue;
        }

        public float SkillDamageEnhance
        {
            get { return battleUnitAttribute.skillDmgRate; }
            private set
            {
                battleUnitAttribute.skillDmgRate = value;
            }
        }

        public float SkillDamageEnhanceUpgrade(float value)
        {
            if (value < 0)
            {
               // Debug.LogError("SkillDamageEnhance提升数值不能为负数 " + this.Name + " " + value);
                value = 0;

            }
            //if (value < 0)
            //    throw new Exception("SkillDamageEnhance提升数值不能为负数 " + value);

            SkillDamageEnhance += value;
            return value;
        }

        public float SkillDamageEnhanceReduce(float value)
        {
            //if (value < 0)
            //    throw new Exception("SkillDamageEnhance降低数值不能为负数 " + value);

            if (value < 0)
            {
               // Debug.LogError("SkillDamageEnhanceReduce 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;

            }

            var realValue = Math.Min(value, SkillDamageEnhance); //防止减成负数
            SkillDamageEnhance -= realValue;
            return realValue;
        }

        public float SkillDamageReduce
        {
            get { return battleUnitAttribute.skillDmgAnti; }
            private set
            {
                battleUnitAttribute.skillDmgAnti = value;
            }
        }

        public float SkillDamageReduceUpgrade(float value)
        {
            //if (value < 0)
            //    throw new Exception("SkillDamageReduce提升数值不能为负数 " + value);

            if (value < 0)
            {
               // Debug.LogError("SkillDamageReduceUpgrade 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;

            }

            SkillDamageReduce += value;
            return value;
        }

        public float SkillDamageReduceReduce(float value)
        {
            //if (value < 0)
            //    throw new Exception("SkillDamageReduce降低数值不能为负数 " + value);

            if (value < 0)
            {
               // Debug.LogError("SkillDamageReduceReduce 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            var realValue = Math.Min(value, SkillDamageReduce); //防止减成负数
            SkillDamageReduce -= realValue;
            return realValue;
        }

        public float DamageEnhance
        {
            get { return battleUnitAttribute.dmgRate; }
            private set
            {
                battleUnitAttribute.dmgRate = value;
            }
        }

        public float DamageEnhanceUpgrade(float value)
        {
            //if (value < 0)
            //    throw new Exception("DamageEnhance提升数值不能为负数 " + value);

            if (value < 0)
            {
               // Debug.LogError("DamageEnhanceUpgrade 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            DamageEnhance += value;
            return value;
        }

        public float DamageEnhanceReduce(float value)
        {
            //if (value < 0)
            //    throw new Exception("DamageEnhance降低数值不能为负数 " + value);

            if (value < 0)
            {
               // Debug.LogError("DamageEnhanceReduce 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            var realValue = Math.Min(value, DamageEnhance); //防止减成负数
            DamageEnhance -= realValue;
            return realValue;
        }


        public float DamageReduce
        {
            get { return battleUnitAttribute.dmgAnti; }
            private set
            {
                battleUnitAttribute.dmgAnti = value;
            }
        }

        public float DamageReduceUpgrade(float value)
        {
            //if (value < 0)
            //    throw new Exception("DamageReduce提升数值不能为负数 " + value);

            if (value < 0)
            {
               // Debug.LogError("DamageReduceUpgrade 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            DamageReduce += value;
            return value;
        }

        public float DamageReduceReduce(float value)
        {
            //if (value < 0)
            //    throw new Exception("DamageReduce降低数值不能为负数 " + value);

            if (value < 0)
            {
               // Debug.LogError("DamageReduceReduce 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            var realValue = Math.Min(value, DamageReduce); //防止减成负数
            DamageReduce -= realValue;
            return realValue;
        }

        public float ControlHit
        {
            get { return battleUnitAttribute.debuffHit; }
            private set
            {
                battleUnitAttribute.debuffHit = value;
            }
        }

        public float ControlHitUpgrade(float value)
        {
            //if (value < 0)
            //    throw new Exception("ControlHit提升数值不能为负数 " + value);

            if (value < 0)
            {
               // Debug.LogError("ControlHitUpgrade 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            ControlHit += value;
            return value;
        }

        public float ControlHitReduce(float value)
        {
            //if (value < 0)
            //    throw new Exception("ControlHit降低数值不能为负数 " + value);

            if (value < 0)
            {
               // Debug.LogError("ControlHitReduce 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            var realValue = Math.Min(value, ControlHit); //防止减成负数
            ControlHit -= realValue;
            return realValue;
        }

        public float ControlResistance
        {
            get { return battleUnitAttribute.debuffAnti; }
            private set
            {
                battleUnitAttribute.debuffAnti = value;
            }
        }

        public float ControlResistanceUpgrade(float value)
        {
            //if (value < 0)
            //    throw new Exception("ControlResistance提升数值不能为负数 " + value);

            if (value < 0)
            {
               // Debug.LogError("ControlResistanceUpgrade 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            ControlResistance += value;
            return value;
        }

        public float ControlResistanceReduce(float value)
        {
            //if (value < 0)
            //    throw new Exception("ControlResistance降低数值不能为负数 " + value);

            if (value < 0)
            {
                //Debug.LogError("ControlResistanceReduce 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            var realValue = Math.Min(value, ControlResistance); //防止减成负数
            ControlResistance -= realValue;
            return realValue;
        }

        public float Puncture
        {
            get { return battleUnitAttribute.penetrate; }
            private set
            {
                battleUnitAttribute.penetrate = value;
            }
        }

        public float PunctureUpgrade(float value)
        {
            //if (value < 0)
            //    throw new Exception("Puncture提升数值不能为负数 " + value);

            if (value < 0)
            {
               //Debug.LogError("PunctureUpgrade 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            Puncture += value;
            return value;
        }

        public float PunctureReduce(float value)
        {
            //if (value < 0)
            //    throw new Exception("Puncture降低数值不能为负数 " + value);

            if (value < 0)
            {
               // Debug.LogError("PunctureReduce 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            var realValue = Math.Min(value, Puncture); //防止减成负数
            Puncture -= realValue;
            return realValue;
        }

        public float Block
        {
            get { return battleUnitAttribute.block; }
            private set
            {
                battleUnitAttribute.block = value;
            }
        }


        public float BlockUpgrade(float value)
        {
            //if (value < 0)
            //    throw new Exception("Block提升数值不能为负数 " + value);

            if (value < 0)
            {
               // Debug.LogError("BlockUpgrade 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            Block += value;
            return value;
        }

        public float BlockReduce(float value)
        {
            //if (value < 0)
            //    throw new Exception("Block降低数值不能为负数 " + value);

            if (value < 0)
            {
               // Debug.LogError("BlockReduce 提升数值不能为负数 " + this.Name + " " + value);
                value = 0;
            }

            var realValue = Math.Min(value, Block); //防止减成负数
            Block -= realValue;
            return realValue;
        }



        #endregion
    }
}