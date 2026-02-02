using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JFramework
{
    public class BaseBufferManager : IBufferManager
    {
        public event Action<IBuffer,int, float[]> onBufferUpdated;
        public event Action<IBuffer> onBufferAdded;
        public event Action<IBuffer> onBufferRemoved;
        public event Action<IBuffer> onBufferCast;//buff触发效果了

        protected List<IBuffer> buffers = new List<IBuffer>();

        BufferDataSource dataSource = null;

        BufferFactory factory = null;

        public IBattleUnit Owner { get; set; }

        public BaseBufferManager(BufferDataSource dataSource, BufferFactory factory)
        {
            this.dataSource = dataSource;
            this.factory = factory;
        }

        /// <summary>
        /// 添加buff,目前只能给玩家加
        /// </summary>
        /// <param name="bufferId"></param>
        /// <param name="buffArg"></param>
        /// <returns></returns>
        public virtual IBuffer AddBuffer(IBattleUnit caster, IBattleUnit target, int bufferId,  int foldCout = 1)
        {
            //如果是共存的，则直接添加， 如果是叠加的，则获取原有BUFFER对象，添加层数       
            IBuffer buffer = null;

            buffer = GetBuffer(bufferId);
            var foldType = dataSource.GetFoldType(bufferId);

            if (buffer != null)
            {
                if (foldType == BufferFoldType.Fold)//可叠加的,且已经存在的
                {
                    buffer.AddFoldCount(foldCout);
                    onBufferUpdated?.Invoke(buffer, buffer.FoldCount, buffer.Args);
                    return buffer;
                }

                if (foldType == BufferFoldType.Replace) //替换（刷新周期） 
                {
                    //to do:刷新周期
                    //throw new Exception("还没有实现替换类型的buff");

                    buffers.Remove(buffer);
                    onBufferRemoved?.Invoke(buffer);
                    //onBufferUpdated?.Invoke(buffer, buffer.FoldCount, buffer.Args);
                    //return buffer;
                }
            }

            //不可叠加的，可共存的
            string uid = Guid.NewGuid().ToString();

            buffer = factory.Create(caster, bufferId, foldCout);    
            buffer.OnAttach(target);
            buffer.onCast += Buffer_onCast;
            buffers.Add(buffer);

            onBufferAdded?.Invoke(buffer);

            return buffer;
        }

        /// <summary>
        /// buff触发效果了，比如复活，中毒，恢复等
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void Buffer_onCast(IBuffer obj)
        {
            onBufferCast?.Invoke(obj);
        }

        /// <summary>
        /// 删除buffer
        /// </summary>
        /// <param name="uid"></param>
        public bool RemoveBuffer(string uid)
        {
            var buffer = GetBuffer(uid);
            if (buffer == null)
                return false;
           
            var result = buffers.Remove(buffer);

            if(result)
            {
                buffer.OnDettach();
                onBufferRemoved?.Invoke(buffer);
            }

            return result;
        }

        /// <summary>
        /// 清理所有buff，会触发onDettach
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Clear()
        {
            for(int i = buffers.Count -1; i >= 0; i --)
            {
                var buff = buffers[i];
                RemoveBuffer(buff.Uid);
            }
        }

        /// <summary>
        /// 更新帧
        /// </summary>
        /// <param name="frame"></param>
        public void Update(CombatFrame frame)
        {
            try
            {
                for (int i = buffers.Count - 1; i >= 0; i--)
                {
                    var buffer = buffers[i];
                    buffer.Update(frame);

                    //如果buffer失效了，则移除
                    if (!buffer.IsValid())
                    {
                        if (!RemoveBuffer(buffer.Uid))
                            throw new InvalidOperationException("删除buff失败，参数错误" + buffer.Uid);
                    }

                    if (buffer.CanCast())
                        buffer.Cast();
                }
            }
            catch(Exception e)
            {
                //UnityEngine.Debug.LogError("buff更新发生异常，自动跳过");
            }
        }

        /// <summary>
        /// 更新buff
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="arg"></param>
        public void UpdateBuffer(string uid, float[] args)
        {
            var buffer = GetBuffer(uid);
            //Debug.Assert(buffer != null, " buffer is null");
            buffer.Args = args;
            onBufferUpdated?.Invoke(buffer, buffer.FoldCount, buffer.Args);
        }


        /// <summary>
        /// 获取buffer对象
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public IBuffer GetBuffer(string uid)
        {
            return buffers.Where(buffer => buffer.Uid.Equals(uid)).SingleOrDefault();
        }

        /// <summary>
        /// 获取buffer对象
        /// </summary>
        /// <param name="bufferId"></param>
        /// <returns></returns>
        public IBuffer[] GetBuffers(int bufferId)
        {
            return buffers.Where(buffer => buffer.Id.Equals(bufferId)).ToArray();
        }

        /// <summary>
        /// 获取所有buffers
        /// </summary>
        /// <returns></returns>
        public IBuffer[] GetBuffers()
        {
            return buffers.ToArray();
        }

        /// <summary>
        /// 获取首个buff
        /// </summary>
        /// <param name="bufferId"></param>
        /// <returns></returns>
        public IBuffer GetBuffer(int bufferId)
        {
            return buffers.Where(buffer => buffer.Id.Equals(bufferId)).FirstOrDefault();
        }

        /// <summary>
        /// 获取所有buff或者debuff
        /// </summary>
        /// <param name="isBuff"></param>
        /// <returns></returns>
        public IBuffer[] GetBuffers(bool isBuff)
        {
            return buffers.Where(buffer => IsBuff(buffer.Id).Equals(isBuff)).ToArray();
        }

        public bool IsBuff(int buffId)
        {
            return dataSource.IsBuff(buffId);
        }

        /// <summary>
        /// 是否有buff
        /// </summary>
        /// <param name="bufferId"></param>
        /// <returns></returns>
        public bool HasBuffer(int bufferId)
        {
            return buffers.Where(buffer => buffer.Id.Equals(bufferId)).SingleOrDefault() != null;
        }

        /// <summary>
        /// 是否有buff
        /// </summary>
        /// <param name="bufferType"></param>
        /// <returns></returns>
        public bool HasBuffer(BufferTriggerType bufferType)
        {
            return buffers.Where(buffer => GetBufferTriggerType(buffer.Id).Equals(bufferType)).SingleOrDefault() != null;
        }

        BufferTriggerType GetBufferTriggerType(int bufferId)
        {
            return dataSource.GetTriigerType(bufferId);
        }



        ///// <summary>
        ///// 调用指定类型的buff
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="origin"></param>
        ///// <returns></returns>
        //public int CallBuff(BufferType type, int origin)
        //{
        //    var value = origin;
        //    var buffers = this.buffers.Where(buffer => buffer.BufferType.Equals(type)).ToList();
        //    foreach (var buffer in buffers)
        //    {
        //        value = buffer.Buff(value);
        //    }
        //    return value;
        //}

        ///// <summary>
        ///// 获取buff值效果
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="origin"></param>
        ///// <returns></returns>
        //public float CallBuff(BufferType type, float origin)
        //{
        //    var value = origin;
        //    var buffers = this.buffers.Where(buffer => buffer.BufferType.Equals(type)).ToList();
        //    foreach (var buffer in buffers)
        //    {
        //        value = buffer.Buff(value);
        //    }
        //    return value;
        //}
    }
}