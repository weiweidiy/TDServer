using JFramework;
using Newtonsoft.Json;

namespace TiktokGame2Server.Others
{
    public class JsonNetSerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return JsonConvert.SerializeObject(obj);
        }
    }
}