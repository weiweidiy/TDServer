using Game.Share;
using JFramework;
using JFramework.Game;
using System.Reflection.Emit;


namespace TiktokGame2Server.Others
{
    public class TiktokConfigManager : TiktokGenConfigManager
    {
        public TiktokConfigManager(IConfigLoader loader, IDeserializer deserializer) : base(loader, deserializer)
        {
            // 可以在这里添加额外的初始化逻辑
        }

        #region 默认配置相关
        public string[] GetDefaultSamuraiBusinessIds() => new string[] { "1", "2" };
        public int GetDefaultFormationPoint() => 7;
        public string GetDefaultDeplySamuraiBusinessId() => "1";
        public FormationType GetAtkFormationType() => FormationType.FormationAtk;

        public FormationType GetDefFormationType() => FormationType.FormationDef;

        public string GetDefaultFormationBusinessId() => "1"; //长蛇阵

        public string GetDefaultFirstNodeBusinessId() => "1";

        public int GetDefaultHpPoolHp() => 50;
        public int GetDefaultHpPoolMaxHp() => 50;

        public int GetDefaultCurrencyCoin() => 100000;

        public int GetDefaultCurrencyPan() => 100000;

        public int GetDefaultBagSlotCount() => 20;

        public List<string> GetDefaultBuildingsBusinessId() => new List<string>() { "1", "2" };

        public string GetHomeBuildingBusinessId() => "1";

        /// <summary>
        /// 根据武士BusinessId获取默认的SoldierBusinessId
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetDefaultSoldierBusinessId(string samuraiBusinessId)
        {
            var samuraiCfg = Get<SamuraiCfgData>(samuraiBusinessId);
            if (samuraiCfg == null)
            {
                throw new Exception($"SamuraiCfgData not found for businessId: {samuraiBusinessId}");
            }
            return samuraiCfg.SoldierUid;
        }
        #endregion

        #region 关卡相关
        /// <summary>
        /// 获取下一个关卡的BusinessId
        /// </summary>
        /// <param name="levelBusinessId"></param>
        /// <returns></returns>
        public string GetNextLevel(string levelBusinessId)
        {
            var levelCfg = Get<LevelsCfgData>(levelBusinessId);
            return levelCfg.Next;
        }

        /// <summary>
        /// 获取前置关卡的BusinessId
        /// </summary>
        /// <param name="levelBusinessId"></param>
        /// <returns></returns>
        public string GetPreLevel(string levelBusinessId)
        {
            var levelCfg = Get<LevelsCfgData>(levelBusinessId);
            return levelCfg.Pre;
        }
        #endregion

        #region 关卡节点相关

        public string GetLevelNodeAchievementRewardBusinessId(string levelNodeBusinessId, int process)
        {
            return "1"; // to do: 根据关卡节点BusinessId和process获取奖励BusinessId
        }

        public string GetLevelNodeVictoryRewardBusinessId(string levelNodeBusinessId)
        {
            var nodeCfgData = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            return "1";// to do: 根据关卡节点BusinessId获取胜利奖励BusinessId
            //return nodeCfgData.WinRewardUid;
        }

        public int GetMaxAchievementProcess(string levelNodeBusinessId)
        {
            var nodeCfgData = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            var achievements = nodeCfgData.AchievementUid;
            return achievements.Count;
        }

        public string? GetAchievementBusinessId(string levelNodeBusinessId, int process)
        {
            var nodeCfgData = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            var achievements = nodeCfgData.AchievementUid;
            if (achievements == null || achievements.Count == 0)
            {
                return string.Empty; // 没有成就
            }
            if (process < 1 || process > achievements.Count)
            {
                return null;
                //throw new ArgumentOutOfRangeException(nameof(process), "Process must be within the range of achievements.");
            }
            return achievements[process - 1];
        }

        /// <summary>
        /// 判断是否是新关卡的第一个节点
        /// </summary>
        /// <param name="levelNodeBusinessId"></param>
        /// <returns></returns>
        public bool IsNewLevelFirstNode(string levelNodeBusinessId)
        {
            var nodeCfgData = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            var preUid = nodeCfgData.PreUid;
            if (preUid == "0")
                return true;
            var preNode = Get<LevelsNodesCfgData>(preUid);
            return preNode.LevelUid != nodeCfgData.LevelUid;
        }

        /// <summary>
        /// 获取指定关卡第一个节点的BusinessId
        /// </summary>
        /// <param name="levelBusinessId"></param>
        /// <returns></returns>
        public string GetLevelFirstNodeBusinessId(string levelBusinessId)
        {
            var levelNodesCfg = Get<LevelsNodesCfgData>((node) => {
                return node.LevelUid.Equals(levelBusinessId);
            });
            var firstNodeUid = levelNodesCfg[0].LevelUid;
            return firstNodeUid;
        }

        /// <summary>
        /// 验证关卡节点BusinessId是否有效(存在于配置中)
        /// </summary>
        /// <param name="levelNodeBusinessId"></param>
        /// <returns></returns>
        public bool IsValidLevelNode(string levelNodeBusinessId)
        {
            return Get<LevelsNodesCfgData>(levelNodeBusinessId) != null;
        }

        /// <summary>
        /// 获取下一个关卡节点的BusinessId列表
        /// </summary>
        /// <param name="levelNodeBusinessId"></param>
        /// <returns></returns>
        public List<string> GetNextLevelNodes(string levelNodeBusinessId)
        {
            var nodeCfg = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            return nodeCfg.NextUid;
        }

        /// <summary>
        /// 获取前置关卡节点的BusinessId
        /// </summary>
        /// <param name="levelNodeBusinessId"></param>
        /// <returns></returns>
        public string GetPreviousLevelNode(string levelNodeBusinessId)
        {
            var nodeCfg = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            return nodeCfg.PreUid;
        }



        /// <summary>
        /// 获取关卡节点的阵型配置
        /// </summary>
        /// <param name="levelNodeBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string[] GetLevelNodeFormation(string levelNodeBusinessId)
        {
            var nodeCfg = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            if (nodeCfg == null)
            {
                throw new Exception($"LevelNodeCfgData not found for businessId: {levelNodeBusinessId}");
            }
            var formationUid = nodeCfg.FormationUid;
            var foramtionCfg = Get<LevelNodeDeployCfgData>(formationUid);
            return foramtionCfg.UnitsUid.ToArray();
        }

        /// <summary>
        /// 获取关卡节点的战斗场景BusinessId
        /// </summary>
        /// <param name="levelNodeBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetLevelNodeCombatSceneBusinessId(string levelNodeBusinessId)
        {
            var nodeCfg = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            if (nodeCfg == null)
            {
                throw new Exception($"LevelNodeCfgData not found for businessId: {levelNodeBusinessId}");
            }
            return nodeCfg.CombatSceneUid;

        }

        #endregion

        #region 副本阵型单位相关
        /// <summary>
        /// 获取阵型单位的武士id
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public string GetFormationUnitSamuraiBusinessId(string formationUnitBusinessId, LevelNodeUnitsCfgData cfg = null)
        {
            return (cfg?.SamuraiUid) ?? Get<LevelNodeUnitsCfgData>(formationUnitBusinessId).SamuraiUid;
        }

        /// <summary>
        /// 获取阵型单位的兵种id
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public string GetFormationUnitSoldierBusinessId(string formationUnitBusinessId, LevelNodeUnitsCfgData cfg = null)
        {
            return (cfg?.SoldierUid) ?? Get<LevelNodeUnitsCfgData>(formationUnitBusinessId).SoldierUid;
        }

        /// <summary>
        /// 阵型单位的额外攻击力
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitExtraAtk(string formationUnitBusinessId, LevelNodeUnitsCfgData cfg = null)
        {
            return (cfg?.Atk) ?? Get<LevelNodeUnitsCfgData>(formationUnitBusinessId).Atk;
        }

        /// <summary>
        /// 阵型单位的额外防御力
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitExtraDefence(string formationUnitBusinessId, LevelNodeUnitsCfgData cfg = null)
        {
            return (cfg?.Def) ?? Get<LevelNodeUnitsCfgData>(formationUnitBusinessId).Def;
        }

        /// <summary>
        /// 阵型单位的额外速度
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitExtraSpeed(string formationUnitBusinessId, LevelNodeUnitsCfgData cfg = null)
        {
            return (cfg?.Speed) ?? Get<LevelNodeUnitsCfgData>(formationUnitBusinessId).Speed;
        }

        /// <summary>
        /// 阵型单位的额外生命值
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationeUnitExtraHp(string formationUnitBusinessId, LevelNodeUnitsCfgData cfg = null)
        {
            return (cfg?.Hp) ?? Get<LevelNodeUnitsCfgData>(formationUnitBusinessId).Hp;
        }

        /// <summary>
        /// 阵型单位的额外等级
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitExtraLevel(string formationUnitBusinessId, LevelNodeUnitsCfgData cfg = null)
        {
            return (cfg?.Level) ?? Get<LevelNodeUnitsCfgData>(formationUnitBusinessId).Level;
        }

        /// <summary>
        /// 获取阵型单位的性别
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitSex(string formationUnitBusinessId, LevelNodeUnitsCfgData cfg = null)
        {
            var formationUnitCfg = Get<LevelNodeUnitsCfgData>(formationUnitBusinessId);
            return 0;//to do:
        }

        /// <summary>
        /// 获取阵型单位的攻击力
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetFormationUnitAttack(string formationUnitBusinessId, LevelNodeUnitsCfgData cfg = null)
        {
            var soldier = GetFormationUnitSoldierBusinessId(formationUnitBusinessId, cfg);
            var atk = GetSoldierAttack(soldier);
            var extraAtk = GetFormationUnitExtraAtk(formationUnitBusinessId, cfg);
            return atk + extraAtk;
        }

        /// <summary>
        /// 获取阵型单位的防御力
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitDefence(string formationUnitBusinessId, LevelNodeUnitsCfgData cfg = null)
        {
            var soldier = GetFormationUnitSoldierBusinessId(formationUnitBusinessId, cfg);
            var def = GetSoldierDefence(soldier);
            var extraDef = GetFormationUnitExtraDefence(formationUnitBusinessId, cfg);
            return def + extraDef;
        }

        /// <summary>
        /// 获取阵型单位的速度
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitSpeed(string formationUnitBusinessId, LevelNodeUnitsCfgData cfg = null)
        {
            var samurai = GetFormationUnitSamuraiBusinessId(formationUnitBusinessId, cfg);
            var samuraiSpeed = GetSamuraiSpeed(samurai);
            var soldier = GetFormationUnitSoldierBusinessId(formationUnitBusinessId, cfg);
            var soldierSpeed = GetSoldierSpeed(soldier);
            var extraSpeed = GetFormationUnitExtraSpeed(formationUnitBusinessId, cfg);
            return samuraiSpeed + soldierSpeed + extraSpeed;
        }

        /// <summary>
        /// 获取阵型单位的最大生命值
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitMaxHp(string formationUnitBusinessId, LevelNodeUnitsCfgData cfg = null)
        {
            var level = GetFormationUnitExtraLevel(formationUnitBusinessId, cfg);
            var extraHp = GetFormationeUnitExtraHp(formationUnitBusinessId, cfg);
            return FormulaMaxHpByLevel(level) + extraHp;
        }

        /// <summary>
        /// 获取阵型单位的战斗力
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitPower(string formationUnitBusinessId, LevelNodeUnitsCfgData cfg = null)
        {
            var samurai = GetFormationUnitSamuraiBusinessId(formationUnitBusinessId, cfg);
            return GetSamuraiPower(samurai);
        }

        /// <summary>
        /// 获取阵型单位的守备力
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitDef(string formationUnitBusinessId, LevelNodeUnitsCfgData cfg = null)
        {
            var samurai = GetFormationUnitSamuraiBusinessId(formationUnitBusinessId, cfg);
            return GetSamuraiDef(samurai);
        }

        /// <summary>
        /// 获取阵型单位的智力
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitIntel(string formationUnitBusinessId, LevelNodeUnitsCfgData cfg = null)
        {
            var samurai = GetFormationUnitSamuraiBusinessId(formationUnitBusinessId, cfg);
            return GetSamuraiIntel(samurai);
        }

        #endregion

        #region samurai相关
        public bool IsValidSamurai(string samuraiBusinessId)
        {
            return Get<SamuraiCfgData>(samuraiBusinessId) != null;
        }

        public int GetSamuraiRare(string samuraiBusinessId)
        {
            return Get<SamuraiCfgData>(samuraiBusinessId)?.Rare ?? 0;
        }
        /// <summary>
        /// 获取武将作为经验武将时，增加的经验值
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetSamuraiExpAddValue(string businessId)
        {
            return 100; // to do: 读取配置
        }

        /// <summary>
        /// 获取武士的战斗力
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <returns></returns>
        public int GetSamuraiPower(string samuraiBusinessId)
        {
            return Get<SamuraiCfgData>(samuraiBusinessId)?.Power ?? 0;
        }

        /// <summary>
        /// 获取武士守备力
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <returns></returns>
        public int GetSamuraiDef(string samuraiBusinessId)
        {
            return Get<SamuraiCfgData>(samuraiBusinessId)?.Def ?? 0;
        }

        /// <summary>
        /// 获取武士的智力
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <returns></returns>
        public int GetSamuraiIntel(string samuraiBusinessId)
        {
            return Get<SamuraiCfgData>(samuraiBusinessId)?.Intel ?? 0;
        }

        /// <summary>
        /// 获取武士的速度
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <returns></returns>
        public int GetSamuraiSpeed(string samuraiBusinessId)
        {
            return Get<SamuraiCfgData>(samuraiBusinessId)?.Speed ?? 0;
        }

        /// <summary>
        /// 获取武士的性别
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <returns></returns>
        public int GetSamuraiSex(string samuraiBusinessId)
        {
            return 0; // to do: 读取配置
        }

        /// <summary>
        /// 根据武士等级，获取武士解锁的action列表
        /// </summary>
        /// <param name="level"></param>
        /// <param name="smuraiBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<string> GetSamuraiActions(int level, string smuraiBusinessId)
        {
            var result = new List<string>();
            //to do: 根据武士等级和武士BusinessId获取武士解锁的action列表
            //result.Add("1");
            //result.Add("2");
            return result;
        }

        /// <summary>
        /// 获取武士的组别
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <returns></returns>
        public int GetSamuraiGroup(string samuraiBusinessId)
        {
            return Get<SamuraiCfgData>(samuraiBusinessId)?.Group ?? 0;
        }

        #endregion

        #region soldier相关
        /// <summary>
        /// 获取兵种的攻击力
        /// </summary>
        /// <param name="soldierBusinessId"></param>
        /// <returns></returns>
        public int GetSoldierAttack(string soldierBusinessId)
        {
            return Get<SoldiersCfgData>(soldierBusinessId)?.Atk ?? 0;
        }

        /// <summary>
        /// 获取兵种的防御力
        /// </summary>
        /// <param name="soldierBusinessId"></param>
        /// <returns></returns>
        public int GetSoldierDefence(string soldierBusinessId)
        {
            return Get<SoldiersCfgData>(soldierBusinessId)?.Def ?? 0;
        }

        /// <summary>
        /// 获取兵种速度
        /// </summary>
        /// <param name="soldierBusinessId"></param>
        /// <returns></returns>
        public int GetSoldierSpeed(string soldierBusinessId)
        {
            return Get<SoldiersCfgData>(soldierBusinessId)?.Speed ?? 0;
        }

        /// <summary>
        /// 获取兵种的技能列表
        /// </summary>
        /// <param name="soldierBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string[] GetSoldierActions(string soldierBusinessId)
        {
            var soldierCfg = Get<SoldiersCfgData>(soldierBusinessId);
            if (soldierCfg == null)
            {
                throw new Exception($"SoldiersCfgData not found for businessId: {soldierBusinessId}");
            }
            return soldierCfg.Actions.ToArray();
        }

        /// <summary>
        /// 获取兵种类型
        /// </summary>
        /// <param name="soldierBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public SoldierType GetSoldierType(string soldierBusinessId)
        {
            var soldierCfg = Get<SoldiersCfgData>(soldierBusinessId);
            if (soldierCfg == null)
            {
                throw new Exception($"SoldiersCfgData not found for businessId: {soldierBusinessId}");
            }
            return (SoldierType)soldierCfg.SoldierType;
        }
        #endregion

        #region action相关

        /// <summary>
        /// 获取触发器名字
        /// </summary>
        /// <param name="triggerBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetTriggerName(string triggerBusinessId)
        {
            var triggerCfg = Get<ActionTriggersCfgData>(triggerBusinessId);
            if (triggerCfg == null)
            {
                throw new Exception($"ActionsTriggersCfgData not found for businessId: {triggerBusinessId}");
            }
            return triggerCfg.Name;
        }

        /// <summary>
        /// 获取触发器中的查找器uid
        /// </summary>
        /// <param name="triggerBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string? GetTriggerFinderUid(string triggerBusinessId)
        {
            var triggerCfg = Get<ActionTriggersCfgData>(triggerBusinessId);
            if (triggerCfg == null)
            {
                throw new Exception($"ActionsTriggersCfgData not found for businessId: {triggerBusinessId}");
            }
            return triggerCfg.FinderUid == "" ? null : triggerCfg.FinderUid;
        }

        /// <summary>
        /// 获取查找器名字
        /// </summary>
        /// <param name="finderBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetFinderName(string finderBusinessId)
        {
            var finderCfg = Get<ActionFindersCfgData>(finderBusinessId);
            if (finderCfg == null)
            {
                throw new Exception($"ActionsFindersCfgData not found for businessId: {finderBusinessId}");
            }
            return finderCfg.Name;
        }

        /// <summary>
        /// 获取查找器参数
        /// </summary>
        /// <param name="finderBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public float[] GetFinderArgs(string finderBusinessId)
        {
            var finderCfg = Get<ActionFindersCfgData>(finderBusinessId);
            if (finderCfg == null)
            {
                throw new Exception($"ActionsFindersCfgData not found for businessId: {finderBusinessId}");
            }
            return finderCfg.Args.ToArray();
        }

        /// <summary>
        /// 获取执行器名字
        /// </summary>
        /// <param name="executorBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetExecutorName(string executorBusinessId)
        {
            var executorCfg = Get<ActionExecutorsCfgData>(executorBusinessId);
            if (executorCfg == null)
            {
                throw new Exception($"ActionsExecutorsCfgData not found for businessId: {executorBusinessId}");
            }
            return executorCfg.Name;
        }

        /// <summary>
        /// 获取执行器参数
        /// </summary>
        /// <param name="executorBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public float[] GetExecutorArgs(string executorBusinessId)
        {
            var executorCfg = Get<ActionExecutorsCfgData>(executorBusinessId);
            if (executorCfg == null)
            {
                throw new Exception($"ActionsExecutorsCfgData not found for businessId: {executorBusinessId}");
            }
            return executorCfg.Args.ToArray();
        }

        /// <summary>
        /// 获取执行器过滤器的名字
        /// </summary>
        /// <param name="executorBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string? GetExecutorFilterName(string executorBusinessId)
        {
            var executorCfg = Get<ActionExecutorsCfgData>(executorBusinessId);
            if (executorCfg == null)
            {
                throw new Exception($"ActionsExecutorsCfgData not found for businessId: {executorBusinessId}");
            }
            return executorCfg.FilterName == "" ? null : executorCfg.FilterName;
        }

        /// <summary>
        /// 获取执行器过滤器的参数
        /// </summary>
        /// <param name="executorBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public float[]? GetExecutorFilterArgs(string executorBusinessId)
        {
            var executorCfg = Get<ActionExecutorsCfgData>(executorBusinessId);
            if (executorCfg == null)
            {
                throw new Exception($"ActionsExecutorsCfgData not found for businessId: {executorBusinessId}");
            }
            return executorCfg.FilterArgs.ToArray();
        }

        /// <summary>
        /// 获取执行器公式名字
        /// </summary>
        /// <param name="executorBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetExecutorFormulaName(string executorBusinessId)
        {
            var executorCfg = Get<ActionExecutorsCfgData>(executorBusinessId);
            if (executorCfg == null)
            {
                throw new Exception($"ActionsExecutorsCfgData not found for businessId: {executorBusinessId}");
            }

            //公式名字不能为空
            if (executorCfg.FormulaName == "")
            {
                throw new Exception($"ActionsExecutorsCfgData FormulaName is empty for businessId: {executorBusinessId}");
            }

            return executorCfg.FormulaName;
        }

        /// <summary>
        /// 获取执行器公式参数
        /// </summary>
        /// <param name="executorBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public float[]? GetExecutorFormulaArgs(string executorBusinessId)
        {
            var executorCfg = Get<ActionExecutorsCfgData>(executorBusinessId);
            if (executorCfg == null)
            {
                throw new Exception($"ActionsExecutorsCfgData not found for businessId: {executorBusinessId}");
            }
            return executorCfg.FormulaArgs.ToArray();
        }

        /// <summary>
        /// 获取action触发器Uid列表
        /// </summary>
        /// <param name="actionBusinessId"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string[]? GetActionTriggersUid(string actionBusinessId, ActionsCfgData cfg = null)
        {
            if (cfg == null)
            {
                cfg = Get<ActionsCfgData>(actionBusinessId);
            }
            if (cfg == null)
            {
                throw new Exception($"ActionsCfgData not found for businessId: {actionBusinessId}");
            }
            var triggersUid = cfg.TriggersUid;
            if (triggersUid == null || triggersUid.Count == 0)
            {
                return null;
            }

            return triggersUid.ToArray();
        }



        /// <summary>
        /// 获取指定action的查找者名称
        /// </summary>
        /// <param name="actionBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string? GetActionFinderUid(string actionBusinessId, ActionsCfgData cfg = null)
        {
            if (cfg == null)
            {
                cfg = Get<ActionsCfgData>(actionBusinessId);
            }

            if (cfg == null)
            {
                throw new Exception($"ActionsCfgData not found for businessId: {actionBusinessId}");
            }

            return cfg.FinderUid == "" ? null : cfg.FinderUid;
        }

        /// <summary>
        /// 获取指定action的执行者uid列表
        /// </summary>
        /// <param name="actionBusinessId"></param>
        /// <param name="index"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string[] GetActionExecutorsUid(string actionBusinessId, ActionsCfgData cfg = null)
        {
            if (cfg == null)
            {
                cfg = Get<ActionsCfgData>(actionBusinessId);
            }
            if (cfg == null)
            {
                throw new Exception($"ActionsCfgData not found for businessId: {actionBusinessId}");
            }

            var executorsUid = cfg.ExecutorsUid;
            if (executorsUid == null || executorsUid.Count == 0)
            {
                throw new Exception($"No executors found for action: {actionBusinessId}");
            }

            return executorsUid.ToArray();
        }

        #endregion

        #region 成就相关
        public string GetAchievementClassName(string achievementBusinessId)
        {
            var achData = Get<AchievementsCfgData>(achievementBusinessId);
            return achData.Name;
        }

        public float[] GetAchievementArgs(string achievementBusinessId)
        {
            var achData = Get<AchievementsCfgData>(achievementBusinessId);
            return achData.Args.ToArray();
        }



        #endregion

        #region 道具相关
        public int GetItemMaxCount(string itemBusinessId)
        {
            return 99;
        }


        #endregion

        #region 奖励相关
        public CurrencyType[]? GetRewardCurrenciesTypes(string rewardBusinessId)
        {
            return new CurrencyType[] { CurrencyType.Coin }; // to do: 读取配置 ， 暂时就给铜钱ss
        }

        public int[]? GetRewardCurrenciesCounts(string rewardBusinessId)
        {
            return new int[] { 100 }; // to do: 读取配置 ， 暂时就给100铜钱
        }

        public string[]? GetRewardItemsBusinessIds(string rewardBusinessId)
        {
            return new string[] { "1" }; // to do: 读取配置 ， 暂时就给1号道具
        }

        public int[]? GetRewardItemsCounts(string rewardBusinessId)
        {
            return new int[] { 1 }; // to do: 读取配置 ， 暂时就给1个1号道具
        }
        #endregion

        #region 抽卡池相关
        public string[] GetSamuraiDrawPool()
        {
            return new string[] { "1", "2", "3" }; // to do: 读取配置
        }

        public (ResourceType, string, int) GetDrawCost(DrawSamuraiPoolType poolType, int count)
        {
            return (ResourceType.Currency, "1", 100 * count); // to do: ���取配置，暂时就给100铜钱
        }





        #endregion

        #region 阵型相关
        /// <summary>
        /// 阵型点位是否解锁
        /// </summary>
        /// <param name="formationBusinessId"></param>
        /// <param name="index"></param>
        /// <param name="homeLevel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool GetFormationPointValidByIndex(string formationBusinessId, int index, int homeLevel)
        {
            var pointData = Get<FormationPointsUnlockCfgData>((i) => i.FormationBusinessId == formationBusinessId && i.PointIndex == index).SingleOrDefault();
            if(pointData == null)
            {
                throw new Exception($"FormationPointsUnlockCfgData not found for formationBusinessId: {formationBusinessId}, index: {index}");
            }
            return pointData.UnlockLevel != -1 && pointData.UnlockLevel <= homeLevel; 
        }
        #endregion

        #region 建筑相关
        /// <summary>
        /// 获取建筑解锁所需等级
        /// </summary>
        /// <param name="buildingBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int GetBuildingUnlockLevel(string buildingBusinessId)
        {
            var buildingCfg = Get<BuildingsCfgData>(buildingBusinessId);
            if (buildingCfg == null)
                throw new Exception($"BuildingsCfgData not found for businessId: {buildingBusinessId}");

            return buildingCfg.UnlockLevel;

        }

        /// <summary>
        /// 是否存在该建筑BusinessId
        /// </summary>
        /// <param name="buildingBusinessId"></param>
        /// <returns></returns>
        public bool HasBuildingBusinessId(string buildingBusinessId)
        {
            var data = Get<BuildingsCfgData>(buildingBusinessId);
            return data != null;
        }

        /// <summary>
        /// 升级所需资源
        /// </summary>
        /// <param name="buildingBusinessId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public (string, int) GetBuildingUpgradeCost(string buildingBusinessId, int level)
        {
            var lst = Get<BuildingsUpgradeCostCfgData>((i) => i.BuildingUid.Equals(buildingBusinessId) && i.BuildingLevel.Equals(level));
            if (lst == null || lst.Count == 0)
            {
                throw new Exception($"No upgrade cost found for building: {buildingBusinessId} at level: {level}");
            }
            if (lst.Count > 1)
            {
                throw new Exception($"Multiple upgrade costs found for building: {buildingBusinessId} at level: {level}");
            }
            return (lst[0].Currency, lst[0].CurrencyCount);
        }

        /// <summary>
        /// 获取建筑参数
        /// </summary>
        /// <param name="buildingBusinessId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int GetBuildingArg(string buildingBusinessId, int level)
        {

            var lst = Get<BuildingsUpgradeCostCfgData>((i) => i.BuildingUid.Equals(buildingBusinessId) && i.BuildingLevel.Equals(level));
            if (lst == null || lst.Count == 0)
            {
                throw new Exception($"No upgrade cost found for building: {buildingBusinessId} at level: {level}");
            }
            if (lst.Count > 1)
            {
                throw new Exception($"Multiple upgrade costs found for building: {buildingBusinessId} at level: {level}");
            }
            return lst[0].Arg;
        }

        #endregion

        #region 公式相关-前后端要保持一致
        /// <summary>
        /// 计算最大血量
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int FormulaMaxHpByLevel(int level)
        {
            return (int)(1000 * (1 + level / 10f)); //to do: 计算最大HP
        }

        public int GetFormulaLevel(int totalExp)
        {
            return ExpCalculator.GetLevelByTotalExp(totalExp);
        }


        public int GetFormulaLevelUpExperience(int level)
        {
            return ExpCalculator.GetLevelUpExp(level);
        }

        public int GetFormulaBuildingUpgradeDuration(string buildingBusinessId, int level)
        {
            // 1级=100秒，60级=7天（604800秒），前期快后期慢
            const int minSeconds = 100;
            const int maxSeconds = 7 * 24 * 3600; // 604800秒
            const int maxLevel = 60;
            const double power = 2.0; // 越大后期越慢
            if (level <= 1) return minSeconds;
            double ratio = (double)(level - 1) / (maxLevel - 1);
            double seconds = minSeconds + (maxSeconds - minSeconds) * Math.Pow(ratio, power);
            return (int)Math.Round(seconds);
        }

        /// <summary>
        /// 获取建筑立即升级的花费公式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public (string, int) GetFormulaBuildingUpgradeImmediatelyCost(double time)
        {
            // 每秒0.1铜钱，向上取整
            int cost = (int)Math.Ceiling(time * 0.1);
            return ("2", cost); // "1"代表铜钱

        }


        #endregion
    }
}