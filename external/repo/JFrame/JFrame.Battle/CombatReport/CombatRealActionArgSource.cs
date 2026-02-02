namespace JFramework
{

    /// <summary>
    /// 模拟unity中的参数
    /// </summary>
    public class CombatRealActionArgSource : CombatActionArgSource
    {
        int actionId;
        public CombatRealActionArgSource(int actionId)
        {
            this.actionId = actionId;
        }

        public override ActionMode GetActionMode()
        {
            switch(actionId)
            {
                case 1:
                    {
                        return ActionMode.Active;
                    }
                case 100:
                    {
                        return ActionMode.Active;
                    }
                case 101:
                    {
                        return ActionMode.Active;
                    }
                default:
                    throw new System.Exception("没有指定mode" + actionId);
            }
        }

        public override ActionType GetActionType()
        {
            switch(actionId)
            {
                case 1:
                    {
                        return ActionType.Skill;
                    }
                case 100:
                    {
                        return ActionType.Normal;
                    }
                case 101:
                    {
                        return ActionType.Skill;
                    }
                default:
                    throw new System.Exception("没有指定mode" + actionId);
            }
        }

        public override float[] GetCdTriggersArgs(int index)
        {
            switch (actionId)
            {
                case 1:
                    {
                        return new float[] { 1 };
                    }
                case 100:
                    {
                        return new float[] { 1 };
                    }
                case 101:
                    {
                        return new float[] { 1 };
                    }
                default:
                    throw new System.Exception("没有指定mode" + actionId);
            }
            
            
        }

        public override int[] GetCdTriggersId()
        {
            switch (actionId)
            {
                case 1:
                    {
                        return new int[] { 3 };
                    }
                case 100:
                    {
                        return new int[] { 3 };
                    }
                case 101:
                    {
                        return new int[] { 3 };
                    }
                default:
                    throw new System.Exception("没有指定mode" + actionId);
            }
        }

        public override int[] GetConditionFindersId()
        {
            return new int[] { 0 };
        }

        public override float[] GetConditionFindersArgs(int index)
        {
            return new float[] { };
        }

        public override float[] GetConditionTriggersArgs(int triggerIndex)
        {
            switch (actionId)
            {
                case 1:
                    {
                        return new float[] { 1,1,3 };
                    }
                case 100:
                    {
                        return new float[] { 1, 1, 3 };
                    }
                case 101:
                    {
                        return new float[] { 1,0 };
                    }
                default:
                    throw new System.Exception("没有指定mode" + actionId);
            }
        }

        public override int[] GetConditionTriggersId()
        {
            switch (actionId)
            {
                case 1:
                    {
                        return new int[] { 1 };
                    }
                case 100:
                    {
                        return new int[] { 1 };
                    }
                case 101:
                    {
                        return new int[] { 4 };
                    }
                default:
                    throw new System.Exception("没有指定mode" + actionId);
            }
        }

        public override float[] GetDelayTriggerArgs()
        {
            switch (actionId)
            {
                case 1:
                    {
                        return new float[] { 0.5f };
                    }
                case 100:
                    {
                        return new float[] { 0.1f };
                    }
                case 101:
                    {
                        return new float[] { 0.1f };
                    }
                default:
                    throw new System.Exception("没有指定mode" + actionId);
            }
        }

        public override int GetDelayTriggerId()
        {
            switch (actionId)
            {
                case 1:
                    {
                        return 3;
                    }
                case 100:
                    {
                        return 3;
                    }
                case 101:
                    {
                        return 3;
                    }
                default:
                    throw new System.Exception("没有指定mode" + actionId);
            }
        }

        public override float[] GetExecutorsArgs(int index)
        {
            switch (actionId)
            {
                case 1:
                    {
                        return new float[] { 0.1f,2 };
                    }
                case 100:
                    {
                        return new float[] { 0.1f, 2 };
                    }
                case 101:
                    {
                        return new float[] { 0.3f, 1.5f };
                    }
                default:
                    throw new System.Exception("没有指定mode" + actionId);
            }
        }

        public override int[] GetExecutorsId()
        {
            switch (actionId)
            {
                case 1:
                    {
                        return new int[] { 1 };
                    }
                case 100:
                    {
                        return new int[] { 1 };
                    }
                case 101:
                    {
                        return new int[] { 3 };
                    }
                default:
                    throw new System.Exception("没有指定mode" + actionId);
            }
        }

        public override float[] GetFindersArgs(int index)
        {
           return new float[] { };
        }

        public override int[] GetFindersId()
        {
            return new int[] { };
        }

        public override int GetFormulaId()
        {
            throw new System.NotImplementedException();
        }

        public override float[] GetFormulaArgs()
        {
            throw new System.NotImplementedException();
        }

        public override int GetActionGroupId()
        {
            throw new System.NotImplementedException();
        }

        public override int GetActionSortId()
        {
            throw new System.NotImplementedException();
        }

        public override float GetBulletSpeed()
        {
            throw new System.NotImplementedException();
        }
    }
}