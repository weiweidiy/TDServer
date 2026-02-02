using System.Collections.Generic;
using System.Linq;

namespace JFramework
{



    /// <summary>
    /// 添加类型
    /// </summary>
    public enum CombatBufferFoldType
    {
        Error,
        Fold, //叠加
        Replace, //替换（刷新周期)
        Union, //共存
    }

    public enum CombatBufferType
    {
        Buffer,
        Debuffer,
    }

    public class CombatBufferManager : UpdateableContainer<BaseCombatBuffer>, ICombatUpdatable
    {
        public override void UpdateWaitingItems()
        {
            //更新
            foreach (var action in waitForUpdateItems)
            {
                Update(action);
            }
            waitForUpdateItems.Clear();

            //删除
            foreach (var item in waitForRemoveItems)
            {
                Remove(item.Uid);
                //item.OnDetach();
            }
            waitForRemoveItems.Clear();

            //添加
            foreach (var item in waitForAddItem)
            {
                switch (item.FoldType)
                {
                    case CombatBufferFoldType.Union: //独立的就直接加
                        {
                            //var buffer = list.Where(i => i.Id == item.Id).SingleOrDefault();
                            //if (buffer != null)
                            //{
                            //    if (buffer.FoldType != CombatBufferFoldType.Union)
                            //        throw new System.Exception(buffer.FoldType + " buff fold type Union 不一致 " + item.Id);
                            //}

                            Add(item);
                        }
                        break;
                    case CombatBufferFoldType.Replace:
                        {
                            var buffer = list.Where(i => i.Id == item.Id).SingleOrDefault();
                            if (buffer != null)
                            {
                                if (buffer.FoldType != CombatBufferFoldType.Replace)
                                    throw new System.Exception(buffer.FoldType + " buff fold type Replace 不一致 " + item.Id);

                                Remove(buffer.Uid);
                            }


                            Add(item);
                        }
                        break;
                    case CombatBufferFoldType.Fold: //叠加
                        {
                            var buffer = list.Where(i => i.Id == item.Id).SingleOrDefault();
                            if (buffer != null)
                            {
                                if (buffer.FoldType != CombatBufferFoldType.Fold)
                                    throw new System.Exception(buffer.FoldType + " buff fold type Fold 不一致 " + item.Id);

                                item.Uid = buffer.Uid;//uid必须一样，否则找不到
                                item.SetCurFoldCount(item.GetCurFoldCount() + buffer.GetCurFoldCount());
                                Update(item);
                            }
                            else
                                Add(item);
                        }
                        break;
                    default:
                        throw new System.Exception($"没有实现buffer foldtype {item.FoldType}");
                }

            }
            waitForAddItem.Clear();
        }






        public void Update(CombatFrame frame)
        {
            foreach (var buffer in GetAll())
            {
                buffer.Update(frame);

                if (buffer.Expired)
                {
                    RemoveItem(buffer);
                }
            }

            UpdateWaitingItems();
        }


    }

}