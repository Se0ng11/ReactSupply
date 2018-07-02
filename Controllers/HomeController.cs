using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactSupply.Bundles;
using ReactSupply.Interface;
using ReactSupply.Logic;
using ReactSupply.Models.DB;
using ReactSupply.Models.Entity;
using System.Threading.Tasks;

namespace ReactSupply.Controllers
{

    [Authorize]
    [Produces("application/json")]
    [Route("api/Home")]
    public class HomeController : BaseController
    {

        public HomeController(SupplyChainContext context,
            ILogger<HistoryController> logger)
            :base(context)
        {
        }

        [HttpGet("[action]")]
        public string GetSupplyRecord()
        {
            var obj = new ConfigurationMainLogic(_context);
            var obj1 = new SupplyRecordLogic(_context);
            var data = obj.SelectHeader();
            var data1 = obj1.SelectAllDataAsync();

            Task.WhenAll(data, data1);

            var result = new TableFormatter { Header = data.Result, Body = data1.Result };
            return Tools.ConvertToJSON(result);
        }

        [HttpGet("[action]")]
        public string GetConfiguration()
        {
            var obj = new ConfigurationMainLogic(_context);
            var data = obj.SelectSchemaHeaderSync();
            var data1 = obj.SelectAllDataAsync();

            Task.WaitAll(data1);

            var result = new TableFormatter { Header = data, Body = data1.Result };

            return Tools.ConvertToJSON(result);
        }


        [HttpPut("[action]")]
        public string PostSingleSupplyRecordField([FromBody]RequestData requestData)
        {
            var obj = new SupplyRecordLogic(_context);
            return PostData(requestData, obj);
        }


        [HttpPut("[action]")]
        public string PostSingleConfigurationField([FromBody]RequestData requestData)
        {
            var obj = new ConfigurationMainLogic(_context);
            return PostData(requestData, obj);
        }
        
        private string PostData([FromBody]RequestData requestData, IConfig config)
        {
            var s = JsonConvert.DeserializeObject(requestData.updated);
            var keyName = ((JObject)s).First.Path;
            var valueName = ((JObject)s).First.Last.ToString();

            var result = config.PostSingleFieldAsync(requestData.identifier, keyName, valueName);

            Task.WaitAll(result);

            return result.Result;
        }

      

    }
}