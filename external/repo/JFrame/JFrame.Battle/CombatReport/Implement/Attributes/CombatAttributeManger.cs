namespace JFramework
{
    /// <summary>
    /// 屬性管理器
    /// </summary>
    public class CombatAttributeManger : ListContainer<IUpdateable>
    {
        /// <summary>
        /// 添加一个加成值
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="value"></param>
        public void PlusExtraValue(CombatAttribute attrType, string uid, double value)
        {
            var item = Get(attrType.ToString());
            var attr =  item as CombatAttributeDouble;
            if (attr == null)
                throw new System.Exception($"AddExtraValue 时没有找到属性 {attrType.ToString()}" );

            attr.AddExtraValue(uid, value);
        }

        /// <summary>
        /// 移除一个加成值
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool RemoveExtraValue(CombatAttribute attrType, string uid)
        {
            var item = Get(attrType.ToString());
            var attr = item as CombatAttributeDouble;
            if (attr == null)
                throw new System.Exception($"AddExtraValue 时没有找到属性 {attrType.ToString()}");

            return attr.RemoveExtraValue(uid);
        }

        /// <summary>
        /// 减少一个加成值，指定数值
        /// </summary>
        /// <param name="attrType"></param>
        /// <param name="uid"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public bool MinusExtraValue(CombatAttribute attrType, string uid, double value)
        {
            var item = Get(attrType.ToString());
            var attr = item as CombatAttributeDouble;
            if (attr == null)
                throw new System.Exception($"AddExtraValue 时没有找到属性 {attrType.ToString()}");

            return attr.MinusExtraValue(uid, value);
        }

        /// <summary>
        /// 重置所有属性
        /// </summary>
        public void ResetAll()
        {
            foreach(var item in GetAll())
            {
                var attr = item as CombatAttributeDouble;
                attr.Reset();
            }
        }
    }

}