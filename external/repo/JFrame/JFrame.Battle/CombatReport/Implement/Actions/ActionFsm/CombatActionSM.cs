using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace JFramework
{
    public class CombatActionSM : BaseSMSync<CombatAction, BaseActionState, ActionSMTrigger>
    {
        ActionInitState actionInitState;
        ActionDisableState actionDisableState;
        ActionReadyState actionReadyState;
        ActionStandbyState actionStandbyState;
        ActionExecutingState actionExecutingState;
        ActionCdingState actionCdingState;
        ActionCrowdControlState actionCrowdControlState;

        protected override List<BaseActionState> GetAllStates()
        {
            var states = new List<BaseActionState>();

            actionInitState = new ActionInitState();
            actionDisableState = new ActionDisableState();
            actionReadyState = new ActionReadyState();
            actionStandbyState = new ActionStandbyState();
            actionExecutingState = new ActionExecutingState();
            actionCdingState = new ActionCdingState();
            actionCrowdControlState = new ActionCrowdControlState();


            states.Add(actionInitState);
            states.Add(actionDisableState);
            states.Add(actionReadyState);
            states.Add(actionStandbyState);
            states.Add(actionExecutingState);
            states.Add(actionCdingState);
            states.Add(actionCrowdControlState);

            return states;
        }

        protected override Dictionary<string, SMConfig> GetConfigs()
        {
            var configs = new Dictionary<string, SMConfig>();

            var initName = actionInitState.Name;
            var initConfig = new SMConfig();
            initConfig.state = actionInitState;
            initConfig.dicPermit = new Dictionary<ActionSMTrigger, BaseActionState>();
            initConfig.dicPermit.Add(ActionSMTrigger.Disable, actionDisableState);
            configs.Add(initName, initConfig);


            var disableName = actionDisableState.Name;
            var disableConfig = new SMConfig();
            disableConfig.state = actionDisableState;
            disableConfig.dicPermit = new Dictionary<ActionSMTrigger, BaseActionState>();
            disableConfig.dicPermit.Add(ActionSMTrigger.Standby, actionStandbyState);  //disable -> standby 开始
            disableConfig.dicPermit.Add(ActionSMTrigger.Cd, actionCdingState);         //disable -> cding  复活
            disableConfig.dicPermit.Add(ActionSMTrigger.ReadyCd, actionReadyState);
            //disableConfig.dicPermit.Add(ActionSMTrigger.Disable, actionDisableState);
            configs.Add(disableName, disableConfig);


            //預置CD
            var readyName = actionReadyState.Name;
            var readyConfig = new SMConfig();
            readyConfig.state = actionReadyState;
            readyConfig.dicPermit = new Dictionary<ActionSMTrigger, BaseActionState>();
            readyConfig.dicPermit.Add(ActionSMTrigger.Standby, actionStandbyState);
            readyConfig.dicPermit.Add(ActionSMTrigger.Disable, actionDisableState);
            configs.Add(readyName, readyConfig);


            //等待状态，等待触发条件满足（触发器触发 + 触发队列空闲）（触发器触发）
            var standbyName = actionStandbyState.Name; 
            var standbyConfig = new SMConfig();
            standbyConfig.state = actionStandbyState;
            standbyConfig.dicPermit = new Dictionary<ActionSMTrigger, BaseActionState>();
            standbyConfig.dicPermit.Add(ActionSMTrigger.Execute, actionExecutingState);  // standby -> executing 释放
            standbyConfig.dicPermit.Add(ActionSMTrigger.Disable, actionDisableState);
            //standbyConfig.dicPermit.Add(ActionSMTrigger.Execute, actionExecutingState);
            configs.Add(standbyName, standbyConfig);

            //执行状态，执行释放，并等待释放完成
            var executingName = actionExecutingState.Name;
            var executingConfig = new SMConfig();
            executingConfig.state = actionExecutingState;
            executingConfig.dicPermit = new Dictionary<ActionSMTrigger, BaseActionState>();
            executingConfig.dicPermit.Add(ActionSMTrigger.Cd, actionCdingState);      // executing -> cding  //释放结束进入cd
            executingConfig.dicPermit.Add(ActionSMTrigger.Disable, actionDisableState);
            //executingConfig.dicPermit.Add(ActionSMTrigger.CrowdControl, actionCrowdControlState); // executing -> crowdControl 被打断
            configs.Add(executingName, executingConfig);

            var cdingName = actionCdingState.Name;
            var cdingConfig = new SMConfig();
            cdingConfig.state = actionCdingState;
            cdingConfig.dicPermit = new Dictionary<ActionSMTrigger, BaseActionState>();
            cdingConfig.dicPermit.Add(ActionSMTrigger.Standby, actionStandbyState);  // cding -> standby 冷却结束，继续等待触发
            cdingConfig.dicPermit.Add(ActionSMTrigger.Disable, actionDisableState);
            configs.Add(cdingName, cdingConfig);


            //var crowControlName = actionCrowdControlState.Name;
            //var crowControlConfig = new SMConfig();
            //crowControlConfig.state = actionCrowdControlState;
            //crowControlConfig.dicPermit = new Dictionary<ActionSMTrigger, BaseActionState>();
            //crowControlConfig.dicPermit.Add(ActionSMTrigger.Cd, actionCdingState);   // crowdControl -> cding
            //configs.Add(crowControlName, crowControlConfig);
            

            return configs;
        }

        public void Update(CombatFrame frame)
        {
            machine.State.Update(frame);
        }

        public void SwitchToDisable()
        {
            machine.Fire(ActionSMTrigger.Disable);
        }

        public void SwitchToReady()
        {
            machine.Fire(ActionSMTrigger.ReadyCd);
        }

        public void SwitchToStandby()
        {
            machine.Fire(ActionSMTrigger.Standby);
        }

        public void SwitchToExecuting()
        {
            machine.Fire(ActionSMTrigger.Execute);
        }

        public void SwitchToCding()
        {
            machine.Fire(ActionSMTrigger.Cd);
        }

        public void SwitchToCrowdControll()
        {
            machine.Fire(ActionSMTrigger.CrowdControl);
        }
    }
}
