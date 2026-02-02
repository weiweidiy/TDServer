using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace JFramework.Common
{
    public class JsonNetChainData : IChainData
    {
        /// <summary>
        /// 当前节点
        /// </summary>
        JToken _jToken;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jToken"></param>
        public JsonNetChainData(JToken jToken)
        {
            _jToken = jToken;
        }

        public override string ToString()
        {
            return _jToken.ToString();
        }

        /// <summary>
        /// 实现索引器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IChainData IChainData.this[object key] => new JsonNetChainData(_jToken[key]);

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="queryCommand">
        /// Operator	Description
        /// $                           The root element to query.This starts all path expressions.
        /// @                           The current node being processed by a filter predicate.
        /// *                           Wildcard.Available anywhere a name or numeric are required.
        /// ..                          Deep scan. Available anywhere a name is required.
        /// .<name>	                    Dot-notated child
        /// ['<name>' (, '<name>')]	    Bracket-notated child or children
        /// [< number > (, < number >)] Array index or indexes
        /// [start:end]                 Array slice operator
        /// [?(<expression>)]	        Filter expression.Expression must evaluate to a boolean value. example:"$[?(@.group == 9 && @.id == 9)].name"</param>
        /// <returns></returns>
        IEnumerable<IChainData> IChainData.SelectMany(string jsonPath)
        {
            IEnumerable<JToken> results = _jToken.SelectTokens(jsonPath);
            int count = results.Count();
            //Console.WriteLine(" count = " + count);
            JsonNetChainData[] arr = new JsonNetChainData[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = new JsonNetChainData(results.ElementAt(i));
            }
            return arr;
        }

        /// <summary>
        /// 筛选数据 
        /// </summary>
        /// <param name="jsonPath">"$[?(@.group == 9 && @.id == 9)].name"</param>
        /// <returns></returns>
        public IChainData Select(string jsonPath)
        {
            JToken result = _jToken.SelectToken(jsonPath);

            return new JsonNetChainData(result);
        }


        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">字段名称</param>
        /// <returns></returns>
        object IChainData.GetValue(object key)
        {
            return _jToken.Value<object>(key);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        object IChainData.GetValue()
        {
            return _jToken.Value<object>();
        }

        public T GetValue<T>(object key)
        {
            return _jToken.Value<T>(key);
        }

        public T GetValue<T>()
        {
            return _jToken.Value<T>();
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="value"></param>
        void IChainData.SetValue(object value)
        {
            var jp = _jToken as JValue;
            if(value is string)
            {
                jp.Value = (string)value; 
            }
            else if(value is int)
            {
                jp.Value = (int)value;
            }
            else if(value is bool)
            {
                jp.Value = (bool)value;
            }
            else if(value is float)
            {
                jp.Value = (float)value;
            }
            else if(value is double)
            {
                jp.Value = (double)value;
            }
            else if (value is uint)
            {
                jp.Value = (uint)value;
            }
            else if(value is ushort)
            {
                jp.Value = (ushort)value;
            }
            else if(value is short)
            {
                jp.Value = (short)value;
            }
            else if(value is char)
            {
                jp.Value = (char)value;
            }
            else if(value is long)
            {
                jp.Value = (long)value;
            }
            else if(value is ulong)
            {
                jp.Value = (ulong)value;
            }
            else if(value is decimal)
            {
                jp.Value = (decimal)value;
            }
            else
            {
                throw new NotImplementedException("未实现该类型：" + value.GetType().Name);
            }
        }

        /// <summary>
        /// 添加JsonObject
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddObject(string key, object value)
        {
            JObject jobj = new JObject();
            jobj.Add(key, new JValue(value));
            ((JObject)_jToken).Add(jobj.Properties());
        }

        /// <summary>
        /// 反序列化（和json版本有关，unity版本只能在unity中有效)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public T ToObject<T>(object serializer = null)
        {
            return serializer == null?  _jToken.ToObject<T>() : _jToken.ToObject<T>((JsonSerializer)serializer);
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveObject(string key)
        {
            return ((JObject)_jToken).Remove(key);
        }

        /// <summary>
        /// 删除自身节点
        /// </summary>
        public void RemoveObject()
        {
            ((JObject)_jToken).Remove();
        }


    }
}
