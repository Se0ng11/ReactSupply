using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactSupply.Interface;
using ReactSupply.Logic;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using System.Threading.Tasks;


namespace ReactSupply.Controllers
{
    [Produces("application/json")]
    [Route("api/Home")]
    public class HomeController : Controller
    {
        private readonly SupplyChainContext _configuration;

        public HomeController(SupplyChainContext configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("[action]")]
        public JsonResult GetSupplyRecord()
        {
            var obj = new ConfigurationMainLogic(_configuration);
            var obj1 = new SupplyRecordLogic(_configuration);
            var data = obj.SelectHeader();
            var data1 = obj1.SelectAllData();

            Task.WaitAll(data, data1);

            var result = new { header = data.Result, body = data1.Result };
            return FormatJSON(result);
        }

        [HttpGet("[action]")]
        public string GetConfiguration()
        {
            var obj = new ConfigurationMainLogic(_configuration);
            var data = obj.SelectAllData();

            

            Task.WaitAll(data);

            return data.Result;
        }


        [HttpPut("[action]")]
        public string PostSingleSupplyRecordField([FromBody]RequestData requestData)
        {
            var obj = new SupplyRecordLogic(_configuration);
            return PostData(requestData, obj);
        }


        [HttpPut("[action]")]
        public string PostSingleConfigurationField([FromBody]RequestData requestData)
        {
            var obj = new ConfigurationMainLogic(_configuration);
            return PostData(requestData, obj);
        }
        
        private string PostData([FromBody]RequestData requestData, IConfig config)
        {
            var s = JsonConvert.DeserializeObject(requestData.updated);
            var keyName = ((JObject)s).First.Path;
            var valueName = ((JObject)s).First.Last.ToString();

            var result = config.PostSingleField(requestData.identifier, keyName, valueName);

            Task.WaitAll(result);

            return result.Result;
        }


        private JsonResult FormatJSON(object result)
        {
            return Json(JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }

    }
}