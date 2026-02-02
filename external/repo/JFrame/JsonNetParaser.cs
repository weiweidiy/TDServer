using Newtonsoft.Json.Linq;


namespace JFramework.Common
{
    public class JsonNetParaser : IParaser
    {
        /// <summary>
        /// 将json字符串解析成 JsonChainData
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public IChainData Parase(string jsonString)
        {
            var jo = JToken.Parse(jsonString);
            return new JsonNetChainData(jo);
        }
    }
}
