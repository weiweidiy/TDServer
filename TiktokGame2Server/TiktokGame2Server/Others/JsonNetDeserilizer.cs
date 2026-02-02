using JFramework;
using Newtonsoft.Json;
using System;
using System.Text;

namespace TiktokGame2Server.Others
{

    public class JsonNetDeserilizer : IDeserializer
    {
        public T ToObject<T>(string str)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentNullException(nameof(str));
            return JsonConvert.DeserializeObject<T>(str)!;
        }

        public T ToObject<T>(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) throw new ArgumentNullException(nameof(bytes));
            var str = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(str)!;
        }

        public object ToObject(string str, Type type)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentNullException(nameof(str));
            return JsonConvert.DeserializeObject(str, type)!;
        }

        public object ToObject(byte[] bytes, Type type)
        {
            if (bytes == null || bytes.Length == 0) throw new ArgumentNullException(nameof(bytes));
            var str = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject(str, type)!;
        }
    }
}