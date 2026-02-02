using JFramework.BattleReportSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace JFramework
{
    public class CombatUnit : ICombatUnit, ICombatUpdatable, ICombatMovable, IActionOwner, IUpdateable //放容器里需要
    {
        //public event Action<ICombatUnit, ICombatAction, List<ICombatUnit>, float> onActionCast;
        //public event Action<ICombatUnit, ICombatAction, float> onActionStartCD;
        //public event Action<ICombatUnit, ICombatAction, ICombatUnit, ExecuteInfo> onHittingTarget;
        //public event Action<CombatExtraData> onDamaging;
        //public event Action<CombatExtraData> onDamaged;
        //public event Action<ICombatUnit, ICombatAction, ICombatUnit, int> onHealed;
        //public event Action<CombatExtraData> onDead;
        //public event Action<ICombatUnit, ICombatAction, ICombatUnit, int> onRebord;
        //public event Action<ICombatUnit, ICombatAction, ICombatUnit, int> onMaxHpUp;
        //public event Action<ICombatUnit, ICombatAction, ICombatUnit, int> onDebuffAnti;
        //public event Action<ICombatUnit, int, ExecuteInfo> onBufferAdding;
        //public event Action<ICombatUnit, ICombatBuffer> onBufferAdded;
        //public event Action<ICombatUnit, ICombatBuffer> onBufferRemoved;
        //public event Action<ICombatUnit, ICombatBuffer> onBufferCast;
        //public event Action<ICombatUnit, ICombatBuffer, int, float[]> onBufferUpdate;
        public event Action<CombatExtraData> onActionCast;
        public event Action<CombatExtraData> onActionStartCD;
        public event Action<CombatExtraData> onActionCdChanged;
        public event Action<CombatExtraData> onHittingTarget;
        public event Action<CombatExtraData> onHittedTarget; //命中完成后
        public event Action<CombatExtraData> onDamaging;
        public event Action<CombatExtraData> onMiss; //miss
        public event Action<CombatExtraData> onDamaged;
        public event Action<CombatExtraData> onHealed;
        public event Action<CombatExtraData> onDead;
        public event Action<CombatExtraData> onRebord;
        public event Action<CombatExtraData> onMaxHpUp;
        public event Action<CombatExtraData> onDebuffAnti;

        public event Action<CombatExtraData> onBufferAdding;
        public event Action<CombatExtraData> onBufferAdded;
        public event Action<CombatExtraData> onBufferRemoved;
        public event Action<CombatExtraData> onBufferCast;
        public event Action<CombatExtraData> onBufferUpdate;
        public event Action<CombatExtraData> onStartMove;
        public event Action<CombatExtraData> onSpeedChanged;
        public event Action<CombatExtraData> onEndMove;

        public event Action<CombatExtraData> onShootTargetChanged;

        /// <summary>
        /// 唯一id
        /// </summary>
        public string Uid { get; private set; }

        CombatExtraData _extraData;
        public CombatExtraData ExtraData
        {
            get => _extraData;
            set { _extraData = value; }
        }


        /// <summary>
        /// buffer管理器
        /// </summary>
        CombatBufferManager bufferManager;

        /// <summary>
        /// 動作管理器
        /// </summary>
        CombatActionManager actionManager;

        /// <summary>
        /// 屬性管理器
        /// </summary>
        CombatAttributeManger attributeManger;

        /// <summary>
        /// 上下文
        /// </summary>
        CombatContext context;

        /// <summary>
        /// 坐標
        /// </summary>
        CombatVector position;

        /// <summary>
        /// 移動方向速度
        /// </summary>
        CombatVector velocity;

        /// <summary>
        /// 目标点
        /// </summary>
        CombatVector targetPsoition;

        /// <summary>
        /// 是否在移動中
        /// </summary>
        bool isMoving;

        /// <summary>
        /// 单位类型
        /// </summary>
        int unitType; //主类型，子类型

        CombatUnitInfo unitInfo;

        bool isStart;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context"></param>
        /// <param name="actions"></param>
        /// <param name="buffers"></param>
        /// <param name="attributes"></param>
        public void Initialize(CombatUnitInfo unitInfo, CombatContext context, List<CombatAction> actions, List<BaseCombatBuffer> buffers, CombatAttributeManger attributeManager, float readyCd = 0f)
        {
            Clear();

            this.context = context;
            this.unitInfo = unitInfo;
            Uid = unitInfo.uid;
            unitType = 0; // unitInfo.type;
            unitType |= (int)unitInfo.mainType;
            unitType |= (int)unitInfo.unitSubType;
            actionManager = new CombatActionManager();
            bufferManager = new CombatBufferManager();
            this.attributeManger = attributeManager;

            //创建一个透传参数
            _extraData = new CombatExtraData();
            _extraData.Owner = this; //持有者
            _extraData.Caster = this;//释放者
            _extraData.FoldCount = 1;
            _extraData.Uid = Guid.NewGuid().ToString();
            _extraData.CdDuration = readyCd;
            //_extraData.Value = (double)GetAttributeCurValue(CombatAttribute.ATK); // to do: 移动到action里去定义

            if (actions != null)
            {
                actionManager.AddActions(actions);
                actionManager.Initialize(this);
            }

            if (buffers != null)
                bufferManager.AddRange(buffers);

            actionManager.onItemAdded += ActionManager_onItemAdded;
            actionManager.onItemRemoved += ActionManager_onItemRemoved;
            actionManager.onItemUpdated += ActionManager_onItemUpdated;
            actionManager.onTriggerOn += ActionManager_onTriggerOn;
            actionManager.onStartExecuting += ActionManager_onStartExecuting;
            actionManager.onStartCD += ActionManager_onStartCD;
            actionManager.onCdChanged += ActionManager_onCdChanged;
            actionManager.onHittingTarget += ActionManager_onHittingTarget;
            actionManager.onTargetHittedComplete += ActionManager_onTargetHittedComplete;


            bufferManager.onItemAdded += BufferManager_onItemAdded;
            bufferManager.onItemRemoved += BufferManager_onItemRemoved;
            bufferManager.onItemUpdated += BufferManager_onItemUpdated;
        }







        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            Uid = "";
            bufferManager = null;
            actionManager = null;
            attributeManger = null;
            isMoving = false;
            _extraData = null;
            context = null;
            unitType = 0;
        }



        #region unit接口

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            StartMove();
            StartAction();
            isStart = true;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            StopMove();
            StopAction();
            isStart = false;
        }



        /// <summary>
        /// 更新逻辑
        /// </summary>
        /// <param name="frame"></param>
        public void Update(CombatFrame frame)
        {
            if (IsAlive())
            {
                actionManager.Update(frame);
                bufferManager.Update(frame);
            }
            else
            {
                if (isStart)
                    Stop();
            }
        }

        /// <summary>
        /// 更新坐标
        /// </summary>
        /// <param name="frame"></param>
        public void UpdatePosition(CombatFrame frame)
        {
            if (!IsAlive())
                return;

            if (IsMoving())   //只是自己移动了，其他单位还没有移动
            {
                position += velocity;
                if (GetTargetPostion() == null)
                    return;

                if (Math.Abs(GetTargetPostion().x - position.x) < 0.5f)
                    StopMove();
            }
        }

        /// <summary>
        /// 开始移动
        /// </summary>
        public void StartMove()
        {
            isMoving = true;
            _extraData.Velocity = GetSpeed();
            onStartMove?.Invoke(_extraData);
        }

        /// <summary>
        /// 停止移动
        /// </summary>
        public void StopMove()
        {
            if (!isMoving)
                return;

            isMoving = false;
            onEndMove?.Invoke(_extraData);
        }

        /// <summary>
        /// 开始动作
        /// </summary>
        void StartAction()
        {
            actionManager.Start();
        }

        private void StopAction()
        {
            actionManager.Stop();
        }

        /// <summary>
        /// 判断主类型
        /// </summary>
        /// <param name="mainType"></param>
        /// <returns></returns>
        public virtual bool IsMainType(UnitMainType mainType)
        {
            return (unitType & (int)mainType) != 0;
        }

        /// <summary>
        /// 判断子类型
        /// </summary>
        /// <param name="subType"></param>
        /// <returns></returns>
        public virtual bool IsSubType(UnitSubType subType)
        {
            return (unitType & (int)subType) != 0;
        }

        /// <summary>
        /// 获取目标点位
        /// </summary>
        /// <returns></returns>
        public CombatVector GetTargetPostion()
        {
            return targetPsoition;
        }

        /// <summary>
        /// 設置坐標
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(CombatVector position)
        {
            this.position = position;
        }

        /// <summary>
        /// 設置速度，目前只在初始化时候设置
        /// </summary>
        /// <param name="speed"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetSpeed(CombatVector speed)
        {
            velocity = speed;
        }

        /// <summary>
        /// 设置目标点
        /// </summary>
        /// <param name="position"></param>
        public void SetTargetPosition(CombatVector position)
        {
            targetPsoition = position;
        }

        /// <summary>
        /// 是否移动
        /// </summary>
        /// <returns></returns>
        public bool IsMoving()
        {
            return isMoving;
        }

        /// <summary>
        /// 获取当前坐标
        /// </summary>
        /// <returns></returns>
        public virtual CombatVector GetPosition()
        {
            return position;
        }

        /// <summary>
        /// 获取移动速度
        /// </summary>
        /// <returns></returns>
        public CombatVector GetSpeed()
        {
            return velocity;
        }

        /// <summary>
        /// 单位id
        /// </summary>
        /// <returns></returns>
        public int GetUnitId()
        {
            return unitInfo.id;
        }

        /// <summary>
        /// 是否是主单位
        /// </summary>
        /// <returns></returns>
        public bool IsMainTarget()
        {
            return unitInfo.parent == null;
        }

        public CombatUnitInfo GetParentInfo()
        {
            return unitInfo.parent;
        }
        #endregion

        #region 属性接口

        /// <summary>
        /// 添加一个加成值
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="value"></param>
        public virtual void PlusExtraValue(CombatAttribute attrType, string uid, double value)
        {
            //var item = GetAttributeManager().Get(attrType.ToString());
            //var attr = item as CombatAttributeDouble;
            //if (attr == null)
            //    throw new System.Exception($"AddExtraValue 时没有找到属性 {attrType.ToString()}");

            //attr.AddExtraValue(uid, value);

            GetAttributeManager().PlusExtraValue(attrType, uid, value);
        }

        /// <summary>
        /// 移除一个加成值
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public virtual bool RemoveExtraValue(CombatAttribute attrType, string uid)
        {
            //var item = GetAttributeManager().Get(attrType.ToString());
            //var attr = item as CombatAttributeDouble;
            //if (attr == null)
            //    throw new System.Exception($"AddExtraValue 时没有找到属性 {attrType.ToString()}");

            //return attr.RemoveExtraValue(uid);

            return GetAttributeManager().RemoveExtraValue(attrType, uid);
        }

        /// <summary>
        /// 减少一个附加值
        /// </summary>
        /// <param name="attrType"></param>
        /// <param name="uid"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool MinusExtraValue(CombatAttribute attrType, string uid, double value)
        {
            return GetAttributeManager().MinusExtraValue(attrType, uid, value);
        }

        /// <summary>
        /// 获取当前属性值
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual object GetAttributeCurValue(CombatAttribute attribute)
        {
            if (attributeManger == null)
                throw new Exception("combat unit attributemanager = null ");

            var attr = attributeManger.Get(attribute.ToString());
            if (attr != null)
            {
                if (attr is CombatAttributeLong)
                {
                    var fattr = attr as CombatAttributeLong;
                    return fattr.CurValue;
                }

                if (attr is CombatAttributeDouble)
                {
                    var fattr = attr as CombatAttributeDouble;
                    if (attribute == CombatAttribute.ATK || attribute == CombatAttribute.CurHp || attribute == CombatAttribute.MaxHP)
                        return Math.Max(0, fattr.CurValue);

                    return fattr.CurValue;
                }

                if (attr is CombatAttributeInt)
                {
                    var fattr = attr as CombatAttributeInt;
                    return fattr.CurValue;
                }

            }
            throw new Exception("沒有找到屬性" + attribute.ToString());
        }

        /// <summary>
        /// 是否還活著
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual bool IsAlive()
        {
            var hpAttr = GetAttributeManager().Get(CombatAttribute.CurHp.ToString());
            if (hpAttr != null)
            {
                var attr = hpAttr as CombatAttributeDouble;
                return attr.CurValue > 0;
            }
            throw new Exception("沒有找到Hp屬性");
        }

        /// <summary>
        /// 是否滿血
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool IsHpFull()
        {
            var hpAttr = GetAttributeManager().Get(CombatAttribute.CurHp.ToString());
            var maxHpAttr = GetAttributeManager().Get(CombatAttribute.MaxHP.ToString());
            if (hpAttr != null && maxHpAttr != null)
            {
                var attr = hpAttr as CombatAttributeDouble;
                var attr2 = maxHpAttr as CombatAttributeDouble;
                return attr.CurValue == attr2.CurValue;
            }
            throw new Exception("沒有找到Hp屬性");
        }

        /// <summary>
        /// 获取hp比率
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual double GetHpPercent()
        {
            var hpAttr = GetAttributeManager().Get(CombatAttribute.CurHp.ToString());
            var maxHpAttr = GetAttributeManager().Get(CombatAttribute.MaxHP.ToString());
            if (hpAttr != null && maxHpAttr != null)
            {
                var attr = hpAttr as CombatAttributeDouble;
                var attr2 = maxHpAttr as CombatAttributeDouble;
                return attr.CurValue / attr2.CurValue;
            }
            throw new Exception("沒有找到Hp屬性");
        }

        /// <summary>
        /// 获取属性管理器
        /// </summary>
        /// <returns></returns>
        public virtual CombatAttributeManger GetAttributeManager()
        {
            return attributeManger;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        public virtual CombatAttributeDouble GetAttribute(CombatAttribute attr)
        {
            return attributeManger.Get(attr.ToString()) as CombatAttributeDouble;
        }
        #endregion

        #region action接口

        /// <summary>
        /// 添加action
        /// </summary>
        /// <param name="action"></param>
        public void AddAction(CombatAction action) => actionManager.AddItem(action);

        /// <summary>
        /// 移除action
        /// </summary>
        /// <param name="action"></param>
        public void RemoveAction(CombatAction action) => actionManager.RemoveItem(action);

        /// <summary>
        /// 更新action
        /// </summary>
        /// <param name="action"></param>
        public void UpdateAction(CombatAction action) => actionManager.UpdateItem(action);

        /// <summary>
        /// 獲取所有action
        /// </summary>
        /// <returns></returns>
        public List<CombatAction> GetActions() => actionManager.GetAll();

        /// <summary>
        /// 获取指定技能
        /// </summary>
        /// <param name="actionUid"></param>
        /// <returns></returns>
        public CombatAction GetAction(string actionUid) => actionManager.Get(actionUid);

        private void ActionManager_onItemUpdated(CombatAction obj)
        {
            //throw new NotImplementedException();
        }

        private void ActionManager_onItemRemoved(CombatAction obj)
        {
            //throw new NotImplementedException();
        }

        private void ActionManager_onItemAdded(IEnumerable<CombatAction> obj)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 触发了
        /// </summary>
        /// <param name="extraData"></param>
        private void ActionManager_onTriggerOn(CombatExtraData extraData)
        {
            if (GetTargetPostion() != null)
                return;

            if (extraData.Action.Mode == ActionMode.Passive)
                return;

            if (isMoving)
                StopMove();
        }

        /// <summary>
        /// action进入cd了
        /// </summary>
        /// <param name="extraData"></param>
        private void ActionManager_onStartCD(CombatExtraData extraData) => onActionStartCD?.Invoke(extraData);

        /// <summary>
        /// action开始释放了
        /// </summary>
        /// <param name="extraData"></param>
        private void ActionManager_onStartExecuting(CombatExtraData extraData) => onActionCast?.Invoke(extraData);

        /// <summary>
        /// 命中前
        /// </summary>
        /// <param name="extraData"></param>
        private void ActionManager_onHittingTarget(CombatExtraData extraData) => onHittingTarget?.Invoke(extraData);

        /// <summary>
        /// 命中后
        /// </summary>
        /// <param name="extraData"></param>
        private void ActionManager_onTargetHittedComplete(CombatExtraData extraData) => onHittedTarget?.Invoke(extraData);

        private void ActionManager_onCdChanged(CombatExtraData extraData) => onActionCdChanged?.Invoke(extraData);

        #endregion

        #region buffer接口

        /// <summary>
        /// 添加一个buffer
        /// </summary>
        /// <param name="buffer"></param>
        public virtual void AddBuffer(BaseCombatBuffer buffer)
        {
            GetBufferManager().AddItem(buffer);

            if (context != null && context.Logger != null)
                context.Logger.Log($"给目标{unitInfo.id} 添加buffer{buffer.Id} + {buffer.GetCurFoldCount()}层  Frame:{context.CombatManager.Frame.CurFrame}");
        }

        /// <summary>
        /// 删除一个buffer
        /// </summary>
        /// <param name="buffer"></param>
        public void RemoveBuffer(BaseCombatBuffer buffer)
        {
            GetBufferManager().RemoveItem(buffer);

            if (context != null && context.Logger != null)
                context.Logger.Log($" 删除目标{unitInfo.id} buffer{buffer.Id} + {buffer.GetCurFoldCount()}层  Frame:{context.CombatManager.Frame.CurFrame}");
        }

        /// <summary>
        /// 更新一个buffer
        /// </summary>
        /// <param name="buffer"></param>
        public void UpdateBuffer(BaseCombatBuffer buffer) => GetBufferManager().UpdateItem(buffer);

        /// <summary>
        /// 查找buffer
        /// </summary>
        /// <param name="bufferType"></param>
        /// <returns></returns>
        public List<BaseCombatBuffer> FindBuffers(CombatBufferType bufferType)
        {
            var result = new List<BaseCombatBuffer>();
            var bufferManager = GetBufferManager();
            foreach (var buffer in bufferManager.GetAll())
            {
                if (buffer.BufferType == bufferType)
                    result.Add(buffer);
            }
            return result;
        }

        private void BufferManager_onItemAdded(IEnumerable<BaseCombatBuffer> buffer)
        {
            foreach (var buf in buffer)
            {
                buf.OnAttach(this);
                onBufferAdded?.Invoke(buf.ExtraData);
            }
        }

        private void BufferManager_onItemUpdated(BaseCombatBuffer buffer)
        {
            //重新启动一下，to do: Restart()
            buffer.OnDetach();
            buffer.OnAttach(this);
            onBufferUpdate?.Invoke(buffer.ExtraData);
        }

        private void BufferManager_onItemRemoved(BaseCombatBuffer buffer)
        {
            buffer.OnDetach();
            onBufferRemoved?.Invoke(buffer.ExtraData);
        }


        public virtual CombatBufferManager GetBufferManager() => bufferManager;


        #endregion

        #region 战斗接口
        public void OnDamage(CombatExtraData extraData)
        {
            //to do: 添加一个预伤害事件，可以修改值
            // onDamaging?.Invoke(hitter, action, this, damage);
            onDamaging?.Invoke(extraData);

            if (extraData.IsMiss)
            {
                onMiss?.Invoke(extraData);
                return;
            }

            var attrManager = GetAttributeManager();
            var hpAttr = attrManager.Get(CombatAttribute.CurHp.ToString());
            if (hpAttr == null)
                throw new Exception("沒有找到Hp屬性 " + Uid);

            var damage = extraData.Value;

            var attr = hpAttr as CombatAttributeDouble;
            if (attr.CurValue <= 0 || damage == 0)
                return;

            attr.Minus(damage);

            //onDamaged?.Invoke(hitter, action, this, damage);
            onDamaged?.Invoke(extraData);

            if (attr.CurValue <= 0)
            {
                OnDead(extraData);
            }
        }

        /// <summary>
        /// 角色死亡了
        /// </summary>
        /// <param name="extraData"></param>
        private void OnDead(CombatExtraData extraData)
        {
            StopMove();

            //所有action設置成非活動狀態，不會出發
            StopAction();

            //to do:清除所有buffer

            //onDead?.Invoke(hitter, action, this);
            onDead?.Invoke(extraData);
        }

        /// <summary>
        /// 收到治疗了
        /// </summary>
        /// <param name="extraData"></param>
        /// <exception cref="Exception"></exception>
        public void OnHeal(CombatExtraData extraData)
        {
            var attrManager = GetAttributeManager();
            var hpAttr = attrManager.Get(CombatAttribute.CurHp.ToString());
            if (hpAttr == null)
                throw new Exception("沒有找到Hp屬性 " + Uid);
            var maxHpAttr = attrManager.Get(CombatAttribute.MaxHP.ToString());
            if (maxHpAttr == null)
                throw new Exception("沒有找到MaxHp屬性 " + Uid);


            var attr = hpAttr as CombatAttributeDouble;
            var attr2 = maxHpAttr as CombatAttributeDouble;

            var hpRecover = (double)extraData.Caster.GetAttributeCurValue(CombatAttribute.HpRecover);
            extraData.Value *= (1 + hpRecover);

            var healValue = Math.Min(extraData.Value, attr2.CurValue - attr.CurValue);
            attr.Plus(healValue);

            extraData.Value = healValue;

            onHealed?.Invoke(extraData);
        }

        public void OnReborn(CombatExtraData extraData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="extraData"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public double OnAttrChanged(CombatExtraData extraData, CombatAttribute attr)
        {
            var itemAttr = GetAttribute(attr);
            if (itemAttr == null)
                throw new Exception($"没有找到属性类型：{attr}");

            double finalValue = 0;
            if (attr == CombatAttribute.ATK || attr == CombatAttribute.MaxHP)
            {
                finalValue = extraData.Value * itemAttr.OriginValue;
            }
            else
            {
                finalValue = extraData.Value; //+ itemAttr.OriginValue;
            }

            var uid = extraData.Action.Uid;

            PlusExtraValue(attr, uid, finalValue);

            if (attr == CombatAttribute.MaxHP)
            {
                var attrManager = GetAttributeManager();
                var hpAttr = attrManager.Get(CombatAttribute.CurHp.ToString());
                var hp = hpAttr as CombatAttributeDouble;
                hp.Plus(finalValue);
            }

            return finalValue;
        }

        /// <summary>
        /// 控制抵抗？
        /// </summary>
        /// <param name="extraData"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnCrowdControlAnti(CombatExtraData extraData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 被控制了
        /// </summary>
        /// <param name="extraData"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnCrowdControled(CombatExtraData extraData)
        {
            //获取正在释放中的持续技能

            //持续技能进入控制状态

        }

        public void OnShootTargetChanged(CombatExtraData extraData)
        {
            onShootTargetChanged?.Invoke(extraData);
        }


        public void Update(IUpdateable value)
        {
            throw new NotImplementedException();
        }

        #endregion


    }




}