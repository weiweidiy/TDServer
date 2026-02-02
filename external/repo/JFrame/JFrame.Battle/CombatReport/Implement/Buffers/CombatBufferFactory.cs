using System;
using System.Collections.Generic;

namespace JFramework
{
    public class CombatBufferFactory : ListContainer<CombatBufferInfo>
    {
        CombatContext context;
        /// <summary>
        /// 预加载所有buffer
        /// </summary>
        /// <param name="buffers"></param>
        /// <param name="context"></param>
        public void PreloadBuffers(List<CombatBufferInfo> buffers, CombatContext context)
        {
            AddRange(buffers);
            this.context = context;
        }

        public virtual BaseCombatBuffer CreateBuffer(int id, CombatExtraData extraData, int foldCount)
        {
            var bufferInfo = Get(id.ToString());

            var buffer = new CombatBuffer();
            buffer.ExtraData = extraData;
            buffer.ExtraData.Buffer = buffer;
            buffer.ExtraData.Uid = Guid.NewGuid().ToString();
            //buffer.SetCurFoldCount(foldCount);
            var actionFactory = new CombatActionFactory();
            buffer.Initialize(bufferInfo, actionFactory.CreateActions(bufferInfo.actionsData, buffer, context), foldCount);
            return buffer;


        }

        ///// <summary>
        ///// 创建buffer
        ///// </summary>
        ///// <param name="bufferInfo"></param>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public BaseCombatBuffer PreloadBuffer(CombatBufferInfo bufferInfo,  CombatContext context)
        //{
        //    var buffer = new CombatBuffer();
        //    buffer.Id = bufferInfo.id;
        //    buffer.Uid = buffer.Id.ToString();
        //    var actionFactory = new CombatActionFactory();
        //    buffer.Initialize(bufferInfo, actionFactory.CreateActions(bufferInfo.actionsData, buffer, context));
        //    return buffer;
        //}



        ///// <summary>
        ///// 获取一个buffer clone
        ///// </summary>
        ///// <param name="bufferId"></param>
        ///// <returns></returns>
        //public virtual BaseCombatBuffer GetBuffer(int bufferId)
        //{
        //    var buffer = Get(bufferId.ToString());

        //    var clone = new CombatBuffer();
        //    clone.Clone(buffer as CombatBuffer);

        //    return clone;
        //}
    }
}