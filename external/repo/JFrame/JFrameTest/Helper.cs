using JFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JFrameTest
{
    public class JsonNetSerializer : IDataConverter
    {
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T ToObject<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        public object ToObject(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        public object ToObject(byte[] bytes, Type type)
        {
            var str = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject(str, type);
        }

        public T ToObject<T>(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
