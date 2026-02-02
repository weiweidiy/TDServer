using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;



namespace JFramework
{
    //public class BaseContainer<T> : IContainer<T> where T : IUnique
    //{
    //    public event Action<T> onItemAdded;
    //    public event Action<T> onItemRemoved;
    //    public event Action<T> onItemUpdated;

    //    List<T> list = new List<T>();
    //    public virtual void Add(T member)
    //    {
    //        list.Add(member);
    //        onItemAdded?.Invoke(member); 
    //    }

    //    public virtual T Get(string uid)
    //    {
    //        return list.Where(i =>  i.Uid == uid).FirstOrDefault();
    //    }

    //    public virtual List<T> GetAll()
    //    {
    //        return list;
    //    }

    //    public virtual bool Remove(string uid)
    //    {
    //        var item = Get(uid);
    //        if(item != null)
    //        {
    //            if(list.Remove(item))
    //            {
    //                onItemRemoved?.Invoke(item);
    //                return true;
    //            }
    //            return false;
    //        }

    //        throw new System.Exception("没有找到要删除的item "  + uid);
    //    }

    //    public virtual void Update(T member)
    //    {
    //        var item = Get(member.Uid);
    //        if(item != null)
    //        {
    //            item = member;
    //            onItemUpdated?.Invoke(item);
    //            return;
    //        }

    //        throw new System.Exception("没有找到要更新的item " + member.Uid);
    //    }

    //    public List<T> Get(Predicate<T> predicate)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public int Count()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}

