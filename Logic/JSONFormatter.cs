using Newtonsoft.Json;

namespace ReactSupply.Logic
{
    public class JSONFormatter
    {
        public string ConvertToJSON(object result)
        {
            return JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
